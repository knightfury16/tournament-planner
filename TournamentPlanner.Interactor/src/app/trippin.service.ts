import { Inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { TRIPPIN_BASE_URL } from './app.config';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Person } from '../../models/trippin/TrippinModel';

@Injectable({
  providedIn: 'root',
})
export class TrippinService {
  constructor(
    private httpClient: HttpClient,
    @Inject(TRIPPIN_BASE_URL) private baseUrl: string
  ) {}

  public getPeople(nameFilter: string): Observable<{ value: Person[] }> {
    let params = new HttpParams();
    if (nameFilter) {
      params = params.set('$filter', `contains(UserName,'${nameFilter}')`);
    }

    params = params.set("$select",'UserName,FirstName,MiddleName,LastName,Age');
    params = params.set("$orderby", 'LastName,FirstName');

    return this.httpClient.get<{ value: Person[] }>(`${this.baseUrl}/People`, {
      params,
    });
  }
}
