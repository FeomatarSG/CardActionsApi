# Card Actions API

Mikroserwis .NET 8 zwracający akcje dozwolone dla karty użytkownika na podstawie typu karty, jej statusu i tego, czy nadano PIN.

Projekt powstał jako zadanie rekrutacyjne (wrzesień 2026). Po otrzymaniu informacji zwrotnej dołożyłem testy end-to-end i dokumentację testów.

## Uruchomienie

```bash
dotnet run --project CardActions.Api
dotnet test CardActions.Api.Tests
```

Swagger: http://localhost:5165/swagger. Przykładowe zapytania: `CardActions.Api/CardActions.Api.http`.

## API

`GET /api/users/User1/cards/Card17/allowed-actions`

```json
{ "cardNumber": "Card17", "allowedActions": ["ACTION3", "ACTION4", "ACTION9"] }
```

- **400** – brak parametru, znaki inne niż litery i cyfry ASCII albo więcej niż 32 znaki
- **404** – karta nie istnieje dla tego użytkownika; ta sama odpowiedź, gdy nie istnieje użytkownik
- **500** – błąd serwisu, ProblemDetails bez szczegółów wewnętrznych

Dostępne karty i ich oczekiwane akcje: [sample-cards.md](sample-cards.md).

## Reguły biznesowe

Akcja jest dozwolona, gdy pozwala na nią **typ karty** i jednocześnie **status karty**. Oba warunki są od siebie niezależne: nigdzie typ nie zmienia reguły dla statusu.

- Typ karty różnicuje tylko jedną akcję, dostępną wyłącznie dla kart kredytowych.
- Dwie akcje zależą dodatkowo od PIN-u, przy czym warunek przypisany jest do pary (akcja, status): ta sama akcja może wymagać nadanego PIN-u przy jednym statusie i jego braku przy innym.
- Wartość typu lub statusu nieznana serwisowi nie daje żadnych akcji.

## Dlaczego wzorzec Specyfikacji

Każda akcja ma jedną specyfikację, czyli obiekt z metodą `IsSatisfiedBy`, złożony z trzech prostych warunków (typ karty, status, PIN) za pomocą `And`, `Or` i `Not`. Dzięki temu cała tabela reguł mieści się w 13 liniach w `CardActionSpecifications`, a reguła każdej akcji jest w jednym miejscu. Nowa akcja to nowy wiersz, a nowy rodzaj warunku (np. limit kredytowy) to nowa mała klasa, bez zmian w pozostałym kodzie. Typy i statusy są wypisane jawnie, więc wartość dodana w przyszłości do enuma jest domyślnie niedozwolona.

## Testy

- **Jednostkowe** – pełna tabela akcji oraz kompletność specyfikacji.
- **Integracyjne** (`WebApplicationFactory`) – odpowiedzi 200, 400, 404 i 500, z podmianą serwisu kart na atrapę.
- **End-to-end** – aplikacja uruchomiona jako osobny proces na prawdziwym serwerze Kestrel, odpytywana przez HTTP jak zwykły klient API.
