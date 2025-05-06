import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { MaterialModule } from './shared/material.module';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([
        (req, next) => {
          console.log('Interceptor called for URL:', req.url);
          const modifiedRequest = req.clone({
            withCredentials: true
          });
          return next(modifiedRequest);
        }
      ])
    ),
    provideAnimations(),
    importProvidersFrom(MaterialModule)
  ]
};
