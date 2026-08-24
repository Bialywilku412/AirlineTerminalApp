PlaneRepository _repository = new PlaneRepository();
PlaneService _service = new PlaneService(_repository);
new DatabaseInitializer().Initialize();
if(_service.ShowAllPlanes().Success == false)
{
    new DatabaseInitializer().AddPlane("Boeing 737", 195);
    new DatabaseInitializer().AddPlane("Airbus 330", 345);
    new DatabaseInitializer().AddPlane("Boeing 787", 215);
}
new DatabaseInitializer().SeedSeats(1);

LoginMenu.ShowLoginMenu();