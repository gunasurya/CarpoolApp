import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, Subject, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Booking } from '../modules/carpooling/components/models/Models';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient, private _snackBar: MatSnackBar) {}

  private baseUrl = 'https://localhost:7094/api/';
  private userUpdatedSource = new Subject<void>();
  userUpdated$ = this.userUpdatedSource.asObservable();

  private bookedRidesAddedSource = new Subject<void>();
  bookedRidesAdded$ = this.bookedRidesAddedSource.asObservable();

  private offeredRidesAddedSource = new Subject<void>();
  offeredRidesAdded$ = this.offeredRidesAddedSource.asObservable();

  // Notify observers that a booked ride has been added
  notifyBookedRideAdded() {
    this.bookedRidesAddedSource.next();
  }

  // Notify observers that an offered ride has been added
  notifyOfferedRideAdded() {
    this.offeredRidesAddedSource.next();
  }

  // Notify observers that user data has been updated
  notifyUserUpdated() {
    this.userUpdatedSource.next();
  }

  private handleHttpError(
    error: HttpErrorResponse,
    defaultMessage: string
  ): Observable<never> {
    console.error(error);
    const errorMessage = error.error?.message || defaultMessage;
    console.error('Error message:', errorMessage);
    return throwError(errorMessage);
  }

  // Get a list of cities
  getCities(): Observable<any[]> {
    const url = `${this.baseUrl}Carpool/cities`;
    return this.http.get<any>(url).pipe(
      map((response: any) => {
        if (response.isSuccess && response.data) {
          return response.data;
        } else {
          throw new Error('Unexpected response format');
        }
      }),
      catchError((error: HttpErrorResponse) => {
        return this.handleHttpError(error, 'Error fetching cities');
      })
    );
  }

  // Add a new user
  addUser(user: any): Observable<void> {
    const url = `${this.baseUrl}User/register`;
    return this.http.post<void>(url, user).pipe(
      map((response: any) => {
        if (response.isSuccess && response.data) {
          return response.data;
        } else if (response.httpStatusCode == 400) {
          throw new Error(response.errorMessage);
        } else {
          throw new Error('Unexpected response format');
        }
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.message === 'Email already taken') {
          return throwError('Email already taken, Failed to Add User!!!'); // Throw a custom error message
        } else {
          return this.handleHttpError(
            error,
            'Failed to add user. Please try again later.'
          );
        }
      })
    );
  }

  // Login a user
  loginUser(user: any): Observable<any> {
    const url = `${this.baseUrl}User/login`;
    return this.http
      .post<{ data: any }>(url, user, { responseType: 'json' })
      .pipe(
        map((response) => {
          if (response.data) {
            return response.data;
          } else {
            throw new Error('Unexpected response format');
          }
        }),
        catchError((error: HttpErrorResponse) => {
          return this.handleHttpError(
            error,
            'Failed to Login user. Please try again later.'
          );
        })
      );
  }

  // Get user by ID
  getUserById(userId: number): Observable<any> {
    const url = `${this.baseUrl}User/${userId}`;
    return this.http.get<any>(url).pipe(
      map((response: any) => {
        if (response.isSuccess && response.data) {
          return response.data;
        } else {
          throw new Error('Unexpected response format');
        }
      }),
      catchError((error: HttpErrorResponse) => {
        return this.handleHttpError(error, 'Error fetching user by ID');
      })
    );
  }

  // Update user data
  updateUser(userId: number, user: any) {
    const url = `${this.baseUrl}User/register/${userId}`;
    return this.http.put<any>(url, user).pipe(
      map((response: any) => {
        if (response.isSuccess && response.data) {
          return response.data;
        } else {
          throw new Error('Unexpected response format');
        }
      }),
      catchError((error: HttpErrorResponse) => {
        return this.handleHttpError(error, 'Error fetching user by ID');
      })
    );
  }

  // Add an offered ride
  addOfferRide(ride: any): Observable<any> {
    const url = `${this.baseUrl}Carpool/offer-ride`;
    return this.http.post<void>(url, ride).pipe(
      map((response: any) => {
        if (response.isSuccess && response.data) {
          return response.data;
        } else {
          throw new Error('Unexpected response format');
        }
      }),
      catchError((error: HttpErrorResponse) => {
        return this.handleHttpError(
          error,
          'Failed to add offer ride. Please try again later.'
        );
      })
    );
  }

  // Get matched rides
  getMatchRides(ride: any): Observable<any> {
    const url = `${this.baseUrl}Carpool/match-rides`;
    return this.http.post<void>(url, ride).pipe(
      map((response: any) => {
        if (response.isSuccess && response.data) {
          return response.data;
        } else {
          throw new Error('Unexpected response format');
        }
      }),
      catchError((error: HttpErrorResponse) => {
        return this.handleHttpError(
          error,
          'Failed to get match rides. Please try again later.'
        );
      })
    );
  }

  // Add a booked ride
  addBookRide(booking: Booking): Observable<any> {
    const url = `${this.baseUrl}Carpool/book-ride`;
    return this.http.post<any>(url, booking).pipe(
      map((response: any) => {
        if (response.isSuccess && response.data) {
          return response.data;
        } else {
          throw new Error('Unexpected response format');
        }
      }),
      catchError((error: HttpErrorResponse) => {
        return this.handleHttpError(
          error,
          'Failed to add book ride. Please try again later.'
        );
      })
    );
  }

  // Get offered rides by user ID
  getOfferedRides(userId: number): Observable<any> {
    const url = `${this.baseUrl}Carpool/offered-rides/${userId}`;
    return this.http.get<any>(url).pipe(
      map((response: any) => {
        if (response.isSuccess && response.data) {
          return response.data;
        } else {
          throw new Error('Unexpected response format');
        }
      }),
      catchError((error: HttpErrorResponse) => {
        return this.handleHttpError(error, 'Error fetching user by ID');
      })
    );
  }

  // Get booked rides by user ID
  getBookedRides(userId: number): Observable<any> {
    const url = `${this.baseUrl}Carpool/booked-rides/${userId}`;
    return this.http.get<any>(url).pipe(
      map((response: any) => {
        if (response.isSuccess && response.data) {
          return response.data;
        } else {
          throw new Error('Unexpected response format');
        }
      }),
      catchError((error: HttpErrorResponse) => {
        return this.handleHttpError(error, 'Error fetching user by ID');
      })
    );
  }

  // Display a snack bar message
  openSnackBar(message: string, action: string = 'ok') {
    this._snackBar.open(message, action, {
      duration: 3000,
      verticalPosition: 'top',
    });
  }
}
