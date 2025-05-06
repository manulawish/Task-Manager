import { HttpErrorResponse } from '@angular/common/http';

export function getErrorMessage(error: HttpErrorResponse): string {
  if (error.error instanceof ErrorEvent) {
    // Client-side error
    return error.error.message;
  }

  // Server-side error
  if (error.status === 401) {
    return 'Your session has expired. Please log in again.';
  }

  if (error.status === 403) {
    return 'You do not have permission to perform this action.';
  }

  if (error.status === 404) {
    return 'The requested resource was not found.';
  }

  if (error.status === 400) {
    // Handle validation errors
    if (typeof error.error === 'object' && error.error.errors) {
      const validationErrors = error.error.errors;
      const errorMessages = Object.keys(validationErrors)
        .map(key => validationErrors[key])
        .flat();
      return errorMessages.join(', ');
    }

    if (typeof error.error === 'string') {
      return error.error;
    }
  }

  if (error.status === 500) {
    return 'A server error occurred. Please try again later.';
  }

  return 'An error occurred. Please try again later.';
}
