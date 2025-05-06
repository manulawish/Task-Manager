import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { TaskService, Task } from '../../services/task.service';
import { CreateTaskDialogComponent } from './create-task-dialog.component';
import { MaterialModule } from '../../shared/material.module';
import { Sort } from '@angular/material/sort';
import { AuthService } from '../../services/auth.service';
import { getErrorMessage } from '../../core/utils/error.utils';

interface TaskForm {
  id: FormControl<number>;
  title: FormControl<string>;
  description: FormControl<string>;
  isCompleted: FormControl<boolean>;
}

@Component({
  selector: 'app-task-index',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule
  ],
  templateUrl: './task-index.component.html',
  styleUrls: ['./task-index.component.css']
})
export class TaskIndexComponent implements OnInit {
  tasks: Task[] = [];
  filteredTasks: Task[] = [];
  displayedColumns: string[] = ['title', 'description', 'isCompleted', 'createdAt', 'actions'];
  taskForm: FormGroup<TaskForm>;
  editingTaskId: number | null = null;
  filterValue: string = '';

  private taskService = inject(TaskService);
  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);
  private authService = inject(AuthService);

  constructor() {
    this.taskForm = this.fb.group<TaskForm>({
      id: new FormControl(0, { nonNullable: true }),
      title: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.minLength(3)] }),
      description: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.minLength(10)] }),
      isCompleted: new FormControl(false, { nonNullable: true })
    });
  }

  ngOnInit(): void {
    this.authService.isLoggedIn$.subscribe(isLoggedIn => {
      if (isLoggedIn) {
        this.loadTasks();
      } else {
        this.router.navigate(['/login']);
      }
    });
  }

  loadTasks(): void {
    this.taskService.getTasks().subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.applyFilter({ target: { value: this.filterValue } } as any);
      },
      error: (error) => {
        if (error.status === 401) {
          this.authService.setLoggedIn(false);
         this.router.navigate(['/login']);
        } else {
          this.snackBar.open('Error loading tasks', 'Close', { duration: 3000 });
        }
      }
    });
  }

  startEdit(task: Task): void {
    this.editingTaskId = task.id;
    this.taskForm.patchValue({
      id: task.id,
      title: task.title,
      description: task.description,
      isCompleted: task.isCompleted
    });
  }

  saveEdit(): void {
    if (this.taskForm.valid && this.editingTaskId) {
      const updatedTask = this.taskForm.value;
      this.taskService.updateTask(this.editingTaskId, updatedTask).subscribe({
        next: () => {
          this.loadTasks();
          this.editingTaskId = null;
          this.taskForm.reset();
          this.snackBar.open('Task updated successfully', 'Close', { duration: 3000 });
        },
        error: (response) => {
          this.snackBar.open(getErrorMessage(response), 'Close', {
            duration: 5000,
            panelClass: ['error-snackbar']
          });
          if (response.status === 401) {
            this.authService.setLoggedIn(false);
            this.router.navigate(['/login']);
          }
        }
      });
    }
  }

  cancelEdit(): void {
    this.editingTaskId = null;
    this.taskForm.reset();
  }

  deleteTask(id: number): void {
    if (confirm('Are you sure you want to delete this task?')) {
      this.taskService.deleteTask(id).subscribe({
        next: () => {
          this.loadTasks();
          this.snackBar.open('Task deleted successfully', 'Close', { duration: 3000 });
        },
        error: (error) => {
          if (error.status === 401) {
            this.authService.setLoggedIn(false);
            this.router.navigate(['/login']);
          } else {
            this.snackBar.open('Error deleting task', 'Close', { duration: 3000 });
          }
        }
      });
    }
  }

  openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateTaskDialogComponent, {
      width: '500px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadTasks();
      }
    });
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterValue = filterValue.trim().toLowerCase();

    this.filteredTasks = this.tasks.filter(task => {
      return task.title.toLowerCase().includes(this.filterValue) ||
             task.description.toLowerCase().includes(this.filterValue);
    });
  }

  sortData(sort: Sort): void {
    const data = this.filteredTasks.slice();
    if (!sort.active || sort.direction === '') {
      this.filteredTasks = data;
      return;
    }

    this.filteredTasks = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'title': return this.compare(a.title, b.title, isAsc);
        case 'description': return this.compare(a.description, b.description, isAsc);
        case 'isCompleted': return this.compare(a.isCompleted, b.isCompleted, isAsc);
        case 'createdAt': return this.compare(new Date(a.createdAt), new Date(b.createdAt), isAsc);
        default: return 0;
      }
    });
  }

  private compare(a: any, b: any, isAsc: boolean): number {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }

  logout(): void {
    this.authService.logout().subscribe({
      next: (response) => {
        console.log('Logout successful:', response);
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.error('Logout error:', error);
        this.snackBar.open('Error during logout', 'Close', { duration: 3000 });
        // Still navigate to login even if there's an error
        this.router.navigate(['/login']);
      }
    });
  }
}
