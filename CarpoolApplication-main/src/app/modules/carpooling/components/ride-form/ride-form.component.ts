import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { City } from '../models/Models';
import { Router } from '@angular/router';
import { AuthGuardService } from 'src/app/services/auth-guard.service';
import { v4 as uuidv4 } from 'uuid';

@Component({
  selector: 'app-ride-form',
  templateUrl: './ride-form.component.html',
  styleUrls: ['./ride-form.component.css'],
})
export class RideFormComponent implements OnInit {
  @Input() buttonText: string = 'Submit';
  @Input() formHeading: string = 'Book a Ride';
  @Input() showFirstSection: boolean = true;
  @Input() showSecondSection: boolean = false;
  @Input() showNextButton?: boolean;
  @Output() formSubmitted = new EventEmitter<FormGroup>();

  firstForm: FormGroup;
  secondForm: FormGroup;
  fullFormData: any;
  perSeatPrice: number = 180;
  cityData: City[] = [];
  userId: any;
  user: any;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router,
    private authService: AuthGuardService
  ) {
    // Initialize the first and second forms with validation
    this.firstForm = this.fb.group({
      from: ['', Validators.required],
      to: ['', Validators.required],
      date: ['', [Validators.required]],
      timeslot: ['', Validators.required],
    });

    this.secondForm = this.fb.group({
      stops: this.fb.array([], [Validators.minLength(1)]),
      availableSeats: ['', Validators.required],
    });
  }

  // Toggle between booking and offering ride
  toggleRideType(event: any) {
    const rideType = event.target.checked;
    if (rideType) {
      this.router.navigate(['/carpool/book-ride']);
    } else {
      this.router.navigate(['/carpool/offer-ride']);
    }
  }

  // Proceed to the next form section
  nextStep() {
    this.showFirstSection = false;
    this.showSecondSection = true;
  }

  // Go back to the first form section
  backToFirstStep() {
    this.showFirstSection = true;
    this.showSecondSection = false;
  }

  // Add a stop in the second form
  addStop() {
    const stops = this.secondForm.get('stops') as FormArray;
    stops.push(this.fb.control('', Validators.required));
  }

  // Get the controls of the stops form array
  get stopsControls() {
    return (this.secondForm.get('stops') as FormArray).controls;
  }

  ngOnInit(): void {
    // Add an initial stop to the form
    this.addStop();

    // Fetch the list of cities from the service
    this.userService.getCities().subscribe(
      (cityData: City[]) => {
        this.cityData = cityData;
      },
      (error) => {
        this.userService.openSnackBar(error);
      }
    );

    // Get the user's ID and user data
    this.userId = this.authService.extractUserData();
    this.userService.getUserById(this.userId).subscribe((res) => {
      this.user = res;
    });
  }

  // Submit the form
  submitForm() {
    const rideId = uuidv4();
    const availableSeatsValue = parseInt(
      this.secondForm.get('availableSeats')?.value
    );
    const fromValue = parseInt(this.firstForm.get('from')?.value);
    const toValue = parseInt(this.firstForm.get('to')?.value);

    // Create the full form data
    this.fullFormData = {
      departureCityId: fromValue,
      destinationCityId: toValue,
      date: this.firstForm.get('date')?.value,
      timeslot: this.firstForm.get('timeslot')?.value,
      stops: this.secondForm.get('stops')?.value.toString(),
      availableSeats: availableSeatsValue,
      fare: (
        this.secondForm.get('availableSeats')?.value * this.perSeatPrice
      ).toString(),
      rideStatus: false,
      driverId: this.userId,
      rideId: rideId,
    };

    if (this.showNextButton) {
      // If it's a multi-step form, emit the first form data
      const modifiedFirstFormGroup = this.fb.group({
        startPoint: fromValue,
        endPoint: toValue,
        date: this.firstForm.get('date')?.value,
        timeslot: this.firstForm.get('timeslot')?.value,
      });

      this.formSubmitted.emit(modifiedFirstFormGroup);
      this.firstForm.reset();
    } else {
      // If it's a single-step form, emit the full form data
      this.formSubmitted.emit(this.fullFormData);
      this.firstForm.reset();
      this.secondForm.reset();
    }
  }
}
