# CodeScape

## Descriere

CodeScape este un proiect realizat in Unity, sub forma unui joc 3D educational. Proiectul contine scene, scripturi si elemente interactive folosite pentru functionalitatile aplicatiei.

## Repository

Codul sursa al proiectului se afla la adresa:

https://gitlab.upt.ro/adelina.jurca/codescape

## Livrabile

Livrabilele proiectului sunt:

- proiectul Unity cu toate fisierele sursa necesare;
- scripturile C# folosite in aplicatie;
- scenele si asset-urile folosite in proiect;
- fisierele de configurare Unity;
- documentatia proiectului;
- acest fisier README, care contine pasii de compilare, instalare si lansare.

## Cerinte

Pentru deschiderea si compilarea proiectului sunt necesare:

- Unity Hub;
- Unity Editor 2022.3.62f3;
- Git, pentru clonarea repository-ului;
- un sistem de operare compatibil cu Unity.

## Descarcarea proiectului

Proiectul poate fi descarcat prin clonarea repository-ului:

```bash
git clone https://gitlab.upt.ro/adelina.jurca/codescape.git
```

Dupa clonare, proiectul se deschide din Unity Hub folosind optiunea `Add project from disk`.

## Compilarea aplicatiei

Pentru compilarea aplicatiei se urmeaza pasii:

1. Se descarca sau se cloneaza repository-ul proiectului.
2. Se deschide Unity Hub.
3. Se alege optiunea `Add project from disk`.
4. Se selecteaza folderul proiectului.
5. Se deschide proiectul folosind Unity Editor 2022.3.62f3.
6. Dupa ce Unity termina importarea fisierelor, se acceseaza meniul `File -> Build Settings`.
7. Se selecteaza platforma dorita, de exemplu `Windows`.
8. Se verifica daca scena principala este adaugata in lista `Scenes In Build`.
9. Se apasa `Build`.
10. Se alege folderul in care va fi generata aplicatia.

## Instalarea aplicatiei

Aplicatia nu necesita instalare separata. Dupa compilare, Unity genereaza un folder care contine fisierul executabil si fisierele necesare rularii.

Daca aplicatia este primita sub forma de arhiva, aceasta trebuie mai intai extrasa intr-un folder local.

## Lansarea aplicatiei

Pentru lansarea aplicatiei se urmeaza pasii:

1. Se deschide folderul in care a fost generat build-ul.
2. Se ruleaza fisierul executabil al aplicatiei, de tip `.exe`.
3. Aplicatia porneste si poate fi utilizata.

## Observatii

In repository nu sunt incluse folderele generate automat de Unity, cum ar fi `Library`, `Temp`, `Obj`, `Logs`, `Build` sau `Builds`, deoarece acestea se regenereaza automat la deschiderea proiectului.
