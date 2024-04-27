import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { AuthGuardService } from 'src/app/services/auth-guard.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  userForm: FormGroup;
  uploadedImageUrl?: string;
  isImageUploaded: boolean = false;
  userId!: number;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private authService: AuthGuardService
  ) {
    // Initialize the user form with validation
    this.userForm = this.fb.group({
      userName: ['', [Validators.pattern(/^[a-zA-Z_-]{3,16}$/)]],
      email: ['', [Validators.email]],
      password: ['', [Validators.minLength(8)]],
    });

    // Disable the form initially
    this.userForm.disable();
  }

  ngOnInit(): void {
    // Get the user's ID from the authentication service
    this.userId = this.authService.extractUserData();

    // Fetch user data based on the ID
    this.userService.getUserById(this.userId).subscribe((res) => {
      const fetchedUserData = res;

      // Populate the form with fetched user data
      this.userForm.patchValue({
        userName: fetchedUserData.userName,
        email: fetchedUserData.email,
      });

      // Set the uploaded image URL if available
      this.uploadedImageUrl = fetchedUserData.image || undefined;
      this.isImageUploaded = !!this.uploadedImageUrl;
    });
  }

  // Handle form submission
  submitForm() {
    const user = {
      image: this.uploadedImageUrl,
      ...this.userForm.value,
    };

    if (this.userForm.valid) {
      // Call API to update user data
      this.userService.updateUser(this.userId, user).subscribe(() => {
        this.userService.notifyUserUpdated();
        this.userService.openSnackBar('User updated successfully');
        this.userForm.disable();
      });
    } else {
      this.userService.openSnackBar(
        'Form is invalid. Please fill in all required fields.'
      );
    }
  }

  // Handle file input change to upload an image
  onChangeFile(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0] || null;

    if (file) {
      const reader = new FileReader();
      reader.addEventListener(
        'load',
        () => {
          this.uploadedImageUrl = reader.result as string;
        },
        false
      );

      this.isImageUploaded = true;
      reader.readAsDataURL(file);
    } else {
      this.isImageUploaded = false;
    }
  }

  // Enable the user form for editing
  enableForm() {
    this.userForm.enable();
  }
}
