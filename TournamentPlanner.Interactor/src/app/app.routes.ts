import { Routes } from '@angular/router';
import { NumberDisplayTestComponent } from './number-display-test/number-display-test.component';

export const routes: Routes = [ 
    {path:'', component:NumberDisplayTestComponent, pathMatch:'full'}
];
