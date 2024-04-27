import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { SignupComponent } from './signup/signup.component';
import { AuthGuard } from './guards/auth.guard';
import { JwtModule } from '@auth0/angular-jwt';


export function tokenGetter() {
  return localStorage.getItem('token');
}

const routes: Routes = [
  { path: 'login', component:LoginComponent  },
  { path: 'signup', component:SignupComponent  },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  {
    path: 'carpool',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./modules/carpooling/carpooling-routing.module').then(
        (m) => m.CarpoolingRoutingModule
      ),
  },
];

const jwt: JwtModule = {
  config: {
    tokenGetter: tokenGetter,
    allowedDomains: ['localhost:7094'],
    disallowedRoutes: [],
  },
};

@NgModule({
  imports: [RouterModule.forRoot(routes), JwtModule.forRoot(jwt)],
  exports: [RouterModule],
  providers: [AuthGuard],
})
export class AppRoutingModule { }
