Create table Thermometer (
	ThermometerId INT PRIMARY KEY,
	CurrentTemp FLOAT NOT NULL,
	MinTemp FLOAT NOT NULL,
	MaxTemp FLOAT NOT NULL,
	Check(MinTemp >= 10 AND MaxTemp <= 30),
	Check(MinTemp  <= MaxTemp)
);