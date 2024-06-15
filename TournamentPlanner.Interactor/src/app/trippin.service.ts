import { Inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { TRIPPIN_BASE_URL } from './app.config';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class TrippinService {
  constructor(
    private httpClient: HttpClient,
    @Inject(TRIPPIN_BASE_URL) private baseUrl: string
  ) {}

  public getPeople(name: string): Observable<{ value: any[] }> {
    return this.httpClient.get<{ value: any[] }>(`${this.baseUrl}/People`);
  }
}
