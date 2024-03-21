# BU2Todo

Todo app created in BU2

1. Skapat planering
2. Skapat github repo
3. Skapa mappstruktur
4. Fixat models

TODO:

1. DbContext // done
2. Controllers ( In progress)
3. Services
4. Endpoints
5. Testing

// Maria anteckningar
// Lägga till user id till alla todos så man ser vilken user som är inloggad
// roller
// updatera todos httpPut
// fler endpoints

// hämta lista med varje användares todos baserat på email 
 1. Varje användare loggar in med en email, och får då ett id -  
 2.  hämtar ut både email och id som är kopplade till varandra, och visar det. 
 3. hämtar ut och filterar alla tillhörande todos till ett specifikt id/email och visar dem.- Och eftersom det är inte bara en information den måste visa, så måste den skapas en egen dto för det som den då hämtar ut.
 