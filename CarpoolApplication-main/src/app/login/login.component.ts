import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { AuthGuardService } from '../services/auth-guard.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  token!: string;
  userId?: number;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private authService: AuthGuardService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Create the login form with email and password fields
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });

    // Check if the user is already logged in, and if so, redirect to the home page
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/carpool/home']);
    }
  }

  submitForm() {
    // Check if the login form is valid
    if (this.loginForm.valid) {
      // Extract email and password from the form
      const loginData = {
        email: this.loginForm.value.email,
        password: this.loginForm.value.password,
      };

      // Call the user service to log in the user
      this.userService.loginUser(loginData).subscribe(
        (res) => {
          // Store the received token and navigate to the home page
          this.token = res;
          this.authService.storeToken(this.token);
          if (this.token) {
            this.router.navigate(['/carpool/home']);
          }
        },
        (error) => {
          // Display an error message if login fails
          this.userService.openSnackBar(error);
        }
      );
    }
  }
}
