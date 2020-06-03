# Readme

Dieses Projekt ist das Backend zu [SafeMarket](https://devpost.com/software/17_supermarkt_status_reservation_system).


## Api dokumentation

`http://localhost:5000/swagger/ui`


## Abhängigkeiten

- Webserver
- MySQL Datenbank

Um das Projekt lauiffähig zu bekommen müssen 2 folgende Daten hinterlegt werden:

`~/MySqlConnect/Data/ReservationContext.cs` - ConnectionString zur Datenbank
`~/WebApi/Services/PlacesDetailService.cs` - GooglePlaces ApiKey
`~/WebApi/Services/PlacesTextsearchResponse.cs` - GooglePlaces ApiKey

## run as docker
docker build -t safemarketimage .
docker run -d -p 5000:8080 --name safemarket safemarketimage


## run with ibm cloud kubernetes 
Follow this instruction with the existing deployment (changes needed) files in kubernetes folder:
https://developer.ibm.com/technologies/containers/tutorials/aspnet-core-app-deployment-in-ibm-cloud-kubernetes-service/
