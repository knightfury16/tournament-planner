import { ApplicationConfig, InjectionToken, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HttpClient, provideHttpClient } from '@angular/common/http';

export const TRIPPIN_BASE_URL = new InjectionToken<string>('TRIPPIN_BASE_URL');

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    {
      provide: TRIPPIN_BASE_URL,
      useValue:
        'https://services.odata.org/TripPinRESTierService/(S(dwkum2ychcndvz35fd2eivpa))',
    },
    // * can use both way to import http client
    // importProvidersFrom(HttpClient),
    provideHttpClient(),
  ],
};
