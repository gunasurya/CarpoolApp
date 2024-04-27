import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root',
})
export class AuthGuardService {
  constructor(private router: Router, private jwtHelper: JwtHelperService) {}

  // Check if the user is currently logged in
  isLoggedIn(): boolean {
    const token = this.getToken();

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      // User is logged in and the token is not expired
      return true;
    } else if (token && this.jwtHelper.isTokenExpired(token)) {
      // Token is expired, so remove it and redirect to the login page
      this.removeToken();
      this.router.navigate(['/login']);
      return false;
    }

    // User is not logged in or has no token
    return false;
  }

  // Get the stored token from local storage
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  // Remove the stored token from local storage
  removeToken(): void {
    localStorage.removeItem('token');
  }

  // Store the provided token value in local storage
  storeToken(tokenValue: string): void {
    localStorage.setItem('token', tokenValue);
  }

  // Log the user out by removing the token and redirecting to the login page
  logOut(): void {
    this.removeToken();
    this.router.navigate(['/login']);
  }

  // Extract user data (the token contains user ID)
  extractUserData(): number {
    const accessToken = localStorage.getItem('token');

    if (accessToken) {
      try {
        const decodedToken = this.jwtHelper.decodeToken(accessToken);
        // Getting user ID from the decoded Token
        const userId = parseInt(decodedToken.UserId);
        return userId;
      } catch (error) {
        console.error('Error decoding token:', error);
      }
    }
    // Return 0 if no user data can be extracted
    return 0;
  }
}
