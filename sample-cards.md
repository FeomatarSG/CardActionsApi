# Karty w danych przykładowych

`CardService` generuje trzech użytkowników (`User1`, `User2`, `User3`), a każdy z nich ma wszystkie
kombinacje typu i statusu karty. Numer karty to `"Card" + numer użytkownika + indeks karty`,
a PIN jest nadany dla parzystych indeksów.

```
#	User1 / User2 / User3		Typ		Status		PIN		Oczekiwane akcje
1	Card11 / Card21 / Card31	Prepaid	Ordered		nie		3, 4, 7, 8, 9, 10, 12, 13
2	Card12 / Card22 / Card32	Prepaid	Inactive	tak		2, 3, 4, 6, 8, 9, 10, 11, 12, 13
3	Card13 / Card23 / Card33	Prepaid	Active		nie		1, 3, 4, 7, 8, 9, 10, 11, 12, 13
4	Card14 / Card24 / Card34	Prepaid	Restricted	tak		3, 4, 9
5	Card15 / Card25 / Card35	Prepaid	Blocked		nie		3, 4, 8, 9
6	Card16 / Card26 / Card36	Prepaid	Expired		tak		3, 4, 9
7	Card17 / Card27 / Card37	Prepaid	Closed		nie		3, 4, 9
8	Card18 / Card28 / Card38	Debit	Ordered		tak		3, 4, 6, 8, 9, 10, 12, 13
9	Card19 / Card29 / Card39	Debit	Inactive	nie		2, 3, 4, 7, 8, 9, 10, 11, 12, 13
10	Card110 / Card210 / Card310	Debit	Active		tak		1, 3, 4, 6, 8, 9, 10, 11, 12, 13
11	Card111 / Card211 / Card311	Debit	Restricted	nie		3, 4, 9
12	Card112 / Card212 / Card312	Debit	Blocked		tak		3, 4, 6, 7, 8, 9
13	Card113 / Card213 / Card313	Debit	Expired		nie		3, 4, 9
14	Card114 / Card214 / Card314	Debit	Closed		tak		3, 4, 9
15	Card115 / Card215 / Card315	Credit	Ordered		nie		3, 4, 5, 7, 8, 9, 10, 12, 13
16	Card116 / Card216 / Card316	Credit	Inactive	tak		2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13
17	Card117 / Card217 / Card317	Credit	Active		nie		1, 3, 4, 5, 7, 8, 9, 10, 11, 12, 13
18	Card118 / Card218 / Card318	Credit	Restricted	tak		3, 4, 5, 9
19	Card119 / Card219 / Card319	Credit	Blocked		nie		3, 4, 5, 8, 9
20	Card120 / Card220 / Card320	Credit	Expired		tak		3, 4, 5, 9
21	Card121 / Card221 / Card321	Credit	Closed		nie		3, 4, 5, 9
```
