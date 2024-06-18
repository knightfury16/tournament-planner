import { Routes } from '@angular/router';
import { NumberDisplayTestComponent } from './number-display-test/number-display-test.component';
import { CashRegisterComponent } from './cash-register/cash-register.component';
import { PeopleListComponent } from './people-list/people-list.component';
import { TripListComponent } from './trip-list/trip-list.component';

export const routes: Routes = [ 
    {path:'', redirectTo:'people', pathMatch:'full'},
    {path:'digit', component:NumberDisplayTestComponent, pathMatch:'full'},
    {path:'cash-register', component:CashRegisterComponent},
    {path:'people', component:PeopleListComponent},
    {path:'people/:userName/trips', component:TripListComponent},
];
