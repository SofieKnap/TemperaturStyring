const form = document.getElementById("limitsForm");
    const minInput = document.getElementById("minTemp");
    const maxInput = document.getElementById("maxTemp");
    const saveButton = document.getElementById("saveButton");
    const message = document.getElementById("message");
    const temperature = document.getElementById("temperature");
    const regulation = document.getElementById("regulation");
    const temperatureFormat = new Intl.NumberFormat("da-DK", {
        maximumFractionDigits: 20
        });

    let thermometerId;

    function showThermometer(thermometer) {
        thermometerId = thermometer.thermometerId;
    minInput.value = thermometer.minTemp;
    maxInput.value = thermometer.maxTemp;

    const currentTemp = temperatureFormat.format(thermometer.currentTemp);
    temperature.textContent = `Aktuel temperatur: ${currentTemp} °C`;

    const heater = thermometer.heaterOn ? "Tændt" : "Slukket";
    const windowState = thermometer.windowOpen ? "Åbent" : "Lukket";
    regulation.textContent =
    `Simuleret status: Varme ${heater.toLowerCase()}, vindue ${windowState.toLowerCase()}.`;
        }

    async function loadThermometer() {
            try {
                const response = await fetch("/api/Thermometer");

    if (!response.ok) {
        message.textContent = "Termometeret kunne ikke hentes.";
    return;
                }

    const [thermometer] = await response.json();

    if (!thermometer) {
        message.textContent = "Der er endnu ikke oprettet et termometer.";
    return;
                }

    showThermometer(thermometer);
    form.hidden = false;
    form.style.removeProperty("display");
            } catch {
        message.textContent = "Termometeret kunne ikke hentes.";
            }
        }

        form.addEventListener("submit", async event => {
        event.preventDefault();

    const minTemp = minInput.valueAsNumber;
    const maxTemp = maxInput.valueAsNumber;

            if (minTemp >= maxTemp) {
        message.textContent =
        "Den nedre grænse skal være mindre end den øvre.";
    return;
            }

    saveButton.disabled = true;
    message.textContent = "";

    try {
                const response = await fetch(
    `/api/Thermometer/${thermometerId}/limits`,
    {
        method: "PUT",
    headers: {"Content-Type": "application/json" },
    body: JSON.stringify({minTemp, maxTemp})
                    }
    );

    if (!response.ok) {
                    const contentType = response.headers.get("content-type") ?? "";
    message.textContent =
    response.status === 400 && contentType.startsWith("text/plain")
    ? await response.text()
    : "Temperaturgrænserne kunne ikke gemmes.";
    return;
                }

    showThermometer(await response.json());
    message.textContent = "Temperaturgrænserne er gemt.";
            } catch {
        message.textContent = "Temperaturgrænserne kunne ikke gemmes.";
            } finally {
        saveButton.disabled = false;
            }
        });

loadThermometer();