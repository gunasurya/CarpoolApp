import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { BookRideComponent } from './components/book-ride/book-ride.component';
import { OfferRideComponent } from './components/offer-ride/offer-ride.component';
import { MyRidesComponent } from './components/my-rides/my-rides.component';
import { ProfileComponent } from './components/profile/profile.component';
import { HomeComponent } from './components/home/home.component';

const routes: Routes = [
  {path:'',component:DashboardComponent,children:[
  {path:'home',component:HomeComponent},
  {path:'book-ride',component:BookRideComponent},
  {path:'offer-ride',component:OfferRideComponent},
  {path:'my-rides',component:MyRidesComponent},
  {path:'profile',component:ProfileComponent},
  {path:'',redirectTo:'/carpool/home',pathMatch:'full'}]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CarpoolingRoutingModule { }
