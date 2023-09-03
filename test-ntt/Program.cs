public class ParkingSystem
{
    private int totalSlots;
    private Dictionary<int, ParkingSlot> parkingSlots;

    public ParkingSystem(int totalSlots)
    {
        this.totalSlots = totalSlots;
        parkingSlots = new Dictionary<int, ParkingSlot>();
        for (int i = 1; i <= totalSlots; i++)
        {
            parkingSlots.Add(i, null);
        }
    }

    public void ParkVehicle(string registrationNo, string color, string vehicleType)
    {
        if (IsParkingFull())
        {
            Console.WriteLine("Sorry, parking lot is full");
            return;
        }

        int slotNumber = GetNextAvailableSlot();
        parkingSlots[slotNumber] = new ParkingSlot(registrationNo, color, vehicleType);
        Console.WriteLine($"Allocated slot number: {slotNumber}");
    }

    public void Leave(int slotNumber)
    {
        if (IsValidSlotNumber(slotNumber))
        {
            parkingSlots[slotNumber] = null;
            Console.WriteLine($"Slot number {slotNumber} is free");
        }
        else
        {
            Console.WriteLine("Invalid slot number");
        }
    }

    public void GetStatus()
    {
        Console.WriteLine("Slot No. Registration No\tType\tColour");
        foreach (var slot in parkingSlots)
        {
            if (slot.Value != null)
            {
                Console.WriteLine($"{slot.Key}\t {slot.Value.RegistrationNo}\t\t{slot.Value.VehicleType}\t{slot.Value.Color}");
            }
        }
    }

    public void GetNumberOfVehiclesByType(string vehicleType)
    {
        int count = parkingSlots.Values
            .Where(slot => slot != null && slot.VehicleType.Equals(vehicleType, StringComparison.OrdinalIgnoreCase))
            .Count();

        Console.WriteLine(count);
    }

    public void GetRegistrationNumbersForVehiclesWithOddPlate()
    {
        var oddPlateNumbers = parkingSlots.Values
            .Where(slot => slot != null && IsOddPlate(slot.RegistrationNo))
            .Select(slot => slot.RegistrationNo)
            .ToList();

        Console.WriteLine(string.Join(", ", oddPlateNumbers));
    }

    public void GetRegistrationNumbersForVehiclesWithEvenPlate()
    {
        var evenPlateNumbers = parkingSlots.Values
            .Where(slot => slot != null && IsEvenPlate(slot.RegistrationNo))
            .Select(slot => slot.RegistrationNo)
            .ToList();

        Console.WriteLine(string.Join(", ", evenPlateNumbers));
    }

    public void GetRegistrationNumbersForVehiclesWithColor(string color)
    {
        var vehiclesWithColor = parkingSlots.Values
            .Where(slot => slot != null && slot.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
            .Select(slot => slot.RegistrationNo)
            .ToList();

        Console.WriteLine(string.Join(", ", vehiclesWithColor));
    }

    public void GetSlotNumbersForVehiclesWithColor(string color)
    {
        var slotsWithColor = parkingSlots
            .Where(slot => slot.Value != null && slot.Value.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
            .Select(slot => slot.Key)
            .ToList();

        Console.WriteLine(string.Join(", ", slotsWithColor));
    }

    public void GetSlotNumberForRegistrationNumber(string registrationNo)
    {
        var slot = parkingSlots.FirstOrDefault(slot => slot.Value != null && slot.Value.RegistrationNo.Equals(registrationNo, StringComparison.OrdinalIgnoreCase));
        if (slot.Value != null)
        {
            Console.WriteLine(slot.Key);
        }
        else
        {
            Console.WriteLine("Not found");
        }
    }

    private bool IsParkingFull()
    {
        return parkingSlots.Values.All(slot => slot != null);
    }

    private int GetNextAvailableSlot()
    {
        return parkingSlots.FirstOrDefault(slot => slot.Value == null).Key;
    }

    private bool IsValidSlotNumber(int slotNumber)
    {
        return slotNumber >= 1 && slotNumber <= totalSlots;
    }

    private bool IsOddPlate(string registrationNo)
    {
        string digits = new string(registrationNo.Where(char.IsDigit).ToArray());

        if (!string.IsNullOrEmpty(digits))
        {
            int lastDigit = int.Parse(digits[digits.Length - 1].ToString());
            return lastDigit % 2 != 0;
        }

        return false;
    }

    private bool IsEvenPlate(string registrationNo)
    {
        string digits = new string(registrationNo.Where(char.IsDigit).ToArray());

        if (!string.IsNullOrEmpty(digits))
        {
            int lastDigit = int.Parse(digits[digits.Length - 1].ToString());
            return lastDigit % 2 == 0;
        }

        return false;
    }
}

public class ParkingSlot
{
    public string RegistrationNo { get; }
    public string Color { get; }
    public string VehicleType { get; }

    public ParkingSlot(string registrationNo, string color, string vehicleType)
    {
        RegistrationNo = registrationNo;
        Color = color;
        VehicleType = vehicleType;
    }
}

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("===( Start Parking Lot )===");
        ParkingSystem parkingSystem = null;

        while (true)
        {
            string command = Console.ReadLine();
            string[] parts = command.Split(' ');

            if (parts[0] == "create_parking_lot")
            {
                int totalSlots = int.Parse(parts[1]);
                parkingSystem = new ParkingSystem(totalSlots);
                Console.WriteLine($"Created a parking lot with {totalSlots} slots");
            }
            else if (parts[0] == "park")
            {
                string registrationNo = parts[1];
                string color = parts[2];
                string vehicleType = parts[3];
                parkingSystem.ParkVehicle(registrationNo, color, vehicleType);
            }
            else if (parts[0] == "leave")
            {
                int slotNumber = int.Parse(parts[1]);
                parkingSystem.Leave(slotNumber);
            }
            else if (parts[0] == "status")
            {
                parkingSystem.GetStatus();
            }
            else if (parts[0] == "type_of_vehicles")
            {
                string type = parts[1];
                parkingSystem.GetNumberOfVehiclesByType(type);
            }
            else if (parts[0] == "registration_numbers_for_vehicles_with_odd_plate")
            {
                parkingSystem.GetRegistrationNumbersForVehiclesWithOddPlate();
            }
            else if (parts[0] == "registration_numbers_for_vehicles_with_even_plate")
            {
                parkingSystem.GetRegistrationNumbersForVehiclesWithEvenPlate();
            }
            else if (parts[0] == "registration_numbers_for_vehicles_with_colour")
            {
                string color = parts[1];
                parkingSystem.GetRegistrationNumbersForVehiclesWithColor(color);
            }
            else if (parts[0] == "slot_numbers_for_vehicles_with_colour")
            {
                string color = parts[1];
                parkingSystem.GetSlotNumbersForVehiclesWithColor(color);
            }
            else if (parts[0] == "slot_number_for_registration_number")
            {
                string registrationNo = parts[1];
                parkingSystem.GetSlotNumberForRegistrationNumber(registrationNo);
            }
            else if (parts[0] == "exit")
            {
                break;
            }
        }

        Console.WriteLine("===( End Parking Lot )===");
    }
}
