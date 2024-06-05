import { Routes } from '@angular/router';
import { GreetingComponent } from './greeting/greeting.component';
import { SayingGoodbyeComponent } from './saying-goodbye/saying-goodbye.component';

export const routes: Routes = [ 
    {path:'', redirectTo:'/greeting', pathMatch:'full'},
    {path: 'greeting', component: GreetingComponent},
    {path:'saying-goodbye', component: SayingGoodbyeComponent}
];
