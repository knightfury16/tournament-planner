import { ApplicationConfig, InjectionToken, importProvidersFrom } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { routes } from './app.routes';
import { HttpClient, provideHttpClient } from '@angular/common/http';
import { IStorageProvider } from '../StorageProviders/IStorageProvider';
import { LocalStorageProvider } from '../StorageProviders/LocalStorageProvider';

export const TRIPPIN_BASE_URL = new InjectionToken<string>('TRIPPIN_BASE_URL');
export const TP_BASE_URL = new InjectionToken<string>("TP_BASE_URL");
export const STORAGE_PROVIDER = new InjectionToken<IStorageProvider>("STORAGE_PROVIDER");


export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withComponentInputBinding()),
    {
      provide: TRIPPIN_BASE_URL,
      useValue:
        'https://services.odata.org/TripPinRESTierService/(S(dwkum2ychcndvz35fd2eivpa))',
    },
    {
      provide: STORAGE_PROVIDER,
      useClass: LocalStorageProvider
    },
    {
      provide: TP_BASE_URL,
      useValue:
      'http://localhost:5151/api'
    },
    // * can use both way to import http client
    // importProvidersFrom(HttpClient),
    provideHttpClient(),
    importProvidersFrom(BrowserAnimationsModule)
  ],
};
