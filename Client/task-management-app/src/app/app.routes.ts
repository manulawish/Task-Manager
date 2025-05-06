import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { LoginComponent } from './features/auth/login/login.component';
import { TaskIndexComponent } from './features/task-index/task-index.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '',
    redirectTo: 'tasks',
    pathMatch: 'full'
  },
  {
    path: '',
    canActivate: [AuthGuard],
    children: [
      { path: 'tasks', component: TaskIndexComponent }
    ]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];
