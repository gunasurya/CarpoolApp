import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CarpoolingRoutingModule } from './carpooling-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { BookRideComponent } from './components/book-ride/book-ride.component';
import { OfferRideComponent } from './components/offer-ride/offer-ride.component';
import { MyRidesComponent } from './components/my-rides/my-rides.component';
import { ProfileComponent } from './components/profile/profile.component';
import { HeaderComponent } from './components/header/header.component';
import { HomeComponent } from './components/home/home.component';
import { RideFormComponent } from './components/ride-form/ride-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/app/services/auth-guard.service'; 

@NgModule({
  declarations: [
   DashboardComponent,
   BookRideComponent,
   OfferRideComponent,
   MyRidesComponent,
   ProfileComponent,
   HeaderComponent,
   HomeComponent,
   RideFormComponent,
  ],
  imports: [
    CommonModule,
    CarpoolingRoutingModule,
    NgbModule,
    ReactiveFormsModule,
    RouterModule
  ],
  providers: [AuthGuardService],
})
export class CarpoolingModule { }
