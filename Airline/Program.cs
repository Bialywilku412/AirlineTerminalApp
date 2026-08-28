PlaneRepository _planeRepository = new PlaneRepository();
PlaneService _planeService = new PlaneService(_planeRepository);

UserRepository _userRepository = new UserRepository();
UserService _userService = new UserService(_userRepository);

FlightRepository _flightRepository = new FlightRepository();
FlightService _flightService = new FlightService(_flightRepository, _planeService);

SeatRepository _seatRepository = new SeatRepository();
SeatService _seatService = new SeatService(_seatRepository);

SeatReservationRepository _seatReservationRepository = new SeatReservationRepository();
SeatReservationService _seatReservationService = new SeatReservationService(_seatReservationRepository, _flightService, _seatService, _planeService);

new DatabaseInitializer().Initialize();
if(_planeService.ShowAllPlanes().Success == false)
{
    new DatabaseInitializer().AddPlane("Boeing 737", 195);
    new DatabaseInitializer().AddPlane("Airbus 330", 345);
    new DatabaseInitializer().AddPlane("Boeing 787", 215);
}
new DatabaseInitializer().SeedSeats(1);

LoginMenu.ShowLoginMenu(_planeService, _userService, _flightService, _seatService, _seatReservationService);