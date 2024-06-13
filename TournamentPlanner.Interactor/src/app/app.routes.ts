import { Routes } from '@angular/router';
import { NumberDisplayTestComponent } from './number-display-test/number-display-test.component';
import { CashRegisterComponent } from './cash-register/cash-register.component';
import { PeopleListComponent } from './people-list/people-list.component';

export const routes: Routes = [ 
    {path:'digit', component:NumberDisplayTestComponent, pathMatch:'full'},
    {path:'cash-register', component:CashRegisterComponent},
    {path:'', component:PeopleListComponent}

];
