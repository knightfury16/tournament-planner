import { ConfigFileOptions, Modes } from "@odata2ts/odata2ts";

const config: ConfigFileOptions = {
  mode:Modes.models,
  services: {
    trippin: {
      sourceUrl: "https://services.odata.org/TripPinRESTierService",
      source: "resource/trippin.xml",
      output: "models/trippin",
    }
  }
}

export default config;