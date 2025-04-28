# Aplikace pro poznámky

Jednoduchá webová aplikace pro správu soukromých poznámek, vytvořená v C# pomocí ASP.NET Core MVC a Entity Framework Core.  
Projekt slouží jako praktická část maturitní zkoušky.

---

## Funkce aplikace

- Registrace uživatele s povinným souhlasem se zpracováním dat
- Přihlášení uživatele
- Přidávání nových poznámek (nadpis + text)
- Výpis poznámek v chronologickém pořadí (od nejnovějších)
- Označování poznámek jako důležité
- Filtrování pouze důležitých poznámek
- Mazání poznámek
- Zrušení uživatelského účtu včetně všech jeho poznámek
- Odhlášení uživatele

---

## Technologie

- **ASP.NET Core MVC** (.NET 8.0)
- **Entity Framework Core**
- **SQL Server** (LocalDB)
- **Session** pro správu přihlášení

---

## Jak projekt spustit

1. **Klonování repozitáře**:
   ```bash
   git clone https://github.com/tvoje_jmeno/poznamkyapp.git
   cd poznamkyapp
