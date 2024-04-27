import { v4 as uuidv4 } from 'uuid';

export class City {
  cityId: number;
  cityName: string;

  constructor(cityId: number, cityName: string) {
    this.cityId = cityId;
    this.cityName = cityName;
  }
}
export class Booking {
  public rideId: any;
  public passengerId: number;
  public pickupLocationId: number;
  public dropLocationId: number;
  public reservedSeats: number;
  public date: string;
  public timeSlot: string;
  public fare: string;
  public bookingId: any;

  constructor(data: any, userId: number) {
    this.rideId = data.rideId;
    this.passengerId = userId;
    this.bookingId = uuidv4();
    this.pickupLocationId = data.departureCityId;
    this.dropLocationId = data.destinationCityId;
    this.reservedSeats = data.availableSeats;
    this.date = data.date;
    this.timeSlot = data.timeSlot;
    this.fare = data.fare;
  }
}
export class Ride {
  public userId: number;
  public startPoint: number;
  public endPoint: number;
  public date: string;
  public timeSlot: string;

  constructor(data: any, userId: number) {
    this.userId = userId;
    this.startPoint = data.startPoint;
    this.endPoint = data.endPoint;
    this.date = data.date;
    this.timeSlot = data.timeslot;
  }
}
