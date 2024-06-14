import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TrippinService {

  constructor() { }

  public getPeople(name: string): Observable<any[]>{
    // request the web and fetch data here
    return of([1,2,3,4,5]);
  }
}
