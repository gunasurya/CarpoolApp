import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
})
export class SignupComponent implements OnInit {
  signupForm!: FormGroup;
  showPassword?: boolean = false;
  showConfirmPassword?: boolean = false;
  userId: number = 0;
  user: any = {};

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService
  ) {
    // Initialize the form when the component is created
    this.initForm();
  }

  ngOnInit(): void {}

  submitForm() {
    // Check if the signup form is valid
    if (this.signupForm.valid) {
      // Extract the confirmPassword field and create a user object without it
      const { confirmPassword, ...userWithoutConfirmPassword } =
        this.signupForm.value;

      // Merge the user object without confirmPassword with the existing user object
      this.user = Object.assign(this.user, userWithoutConfirmPassword);

      // Call the UserService to add a new user
      this.userService.addUser(this.user).subscribe(
        () => {
          // Display a success message and reset the form on successful user addition
          this.userService.openSnackBar('User Added Successfully');
          this.signupForm.reset();
        },
        (error) => {
          // Display an error message if user addition fails
          this.userService.openSnackBar(error);
        }
      );
    }
  }

  initForm() {
    // Create the signup form with email, password, and confirmPassword fields
    this.signupForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: [
        '',
        [
          Validators.required,
          Validators.pattern(
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/
          ),
        ],
      ],
      confirmPassword: ['', Validators.required],
    });
  }

  isPasswordValid() {
    // Check if the password in the form matches a specific regex pattern
    const password = this.signupForm.get('password')?.value;
    const passwordRegex =
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/;
    return passwordRegex.test(password);
  }

  isConfirmPasswordValid() {
    // Check if the confirmPassword in the form matches the password
    const password = this.signupForm.get('password')?.value;
    const confirmPassword = this.signupForm.get('confirmPassword')?.value;
    return password === confirmPassword;
  }

  togglePasswordVisibility(): void {
    // Toggle the visibility of the password field
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    // Toggle the visibility of the confirmPassword field
    this.showConfirmPassword = !this.showConfirmPassword;
  }
}
