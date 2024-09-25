# Inledning

Denna guide syftar till att hjälpa dig att skapa och konfigurera en Docker-baserad ToDo-applikation som körs på en Azure VM med Cosmos DB som databas. För att genomföra stegen i denna guide behöver du följande:

## Krav
- **Azure-konto**: För att provisionera och hantera din virtuella maskin.
- **GitHub-konto**: För att hantera och lagra din kod i ett repository.
- **Docker Hub-konto**: För att lagra och distribuera dina Docker-bilder.

## Rekommenderade verktyg
- **Visual Studio Code**: En kraftfull kodredigerare som underlättar kodhantering och skriptredigering under processen.

Genom att följa dessa steg noggrant säkerställer du att din miljö är korrekt konfigurerad och fungerar som avsett. 

# Guide

### Steg 1: Ladda ner filer från Repository
1. Gå till ditt GitHub-repository.
2. Ladda ner filerna till din lokala maskin:
   - Använd kommandot: 
     ```bash
     git clone <repository-url>
     ```

### Steg 2: Skapa en egen Git Repository
1. Skapa en ny mapp för ditt repository:
   ```bash
   mkdir MyNewRepo
   cd MyNewRepo
   ```
2. Initiera ett nytt Git-repository:
   ```bash
   git init
   ```
3. Kopiera de nedladdade filerna till den nya mappen.
4. Lägg till filerna till Git:
   ```bash
   git add .
   ```
5. Gör en commit:
   ```bash
   git commit -m "Första commit med filer"
   ```
6. Skapa ett nytt repository på GitHub.
7. Koppla ditt lokala repository till GitHub:
   ```bash
   git remote add origin <new-repository-url>
   ```
8. Push-a till GitHub:
   ```bash
   git push -u origin main
   ```

### Steg 3: Logga in på Docker Hub
1. Gå till [Docker Hub](https://hub.docker.com/).
2. Logga in på ditt konto.
3. Gå till "Account Settings".
4. Generera en **read, write, delete** access token och kopiera den.

### Steg 4: Lägg till Docker Token som Secret i GitHub
1. Gå till ditt repository på GitHub.
2. Klicka på **"Settings"**.
3. Gå till **"Secrets and variables"** > **"Actions"**.
4. Klicka på **"New repository secret"**.
5. Lägg till `DOCKER_USERNAME` med ditt Docker Hub-användarnamn.
6. Lägg till `DOCKER_PASSWORD` med den access token som du genererade.

### Steg 5: Skapa en GitHub Actions Workflow
1. Logga in på [GitHub](https://github.com/).
2. Navigera till ditt repository.
3. Klicka på fliken **"Actions"**.
4. Klicka på **"set up workflow yourself"**.
5. Skriv `.github/workflows/ci.yml` i textfältet för filnamn.
6. Klistra in följande YAML-kod i textredigeraren:

   ```yaml
   name: Docker Image CI

   on:
     push:
       branches:
         - main
         
   jobs:
     push_to_registry:
       name: Push Docker image to Docker Hub
       runs-on: ubuntu-latest
       steps:
         - name: Check out the repo
           uses: actions/checkout@v3
         
         - name: Log in to Docker Hub
           uses: docker/login-action@v3.3.0
           with:
             username: ${{ secrets.DOCKER_USERNAME }}
             password: ${{ secrets.DOCKER_PASSWORD }}
         
         - name: Extract metadata (tags, labels) for Docker
           id: meta
           uses: docker/metadata-action@v5.5.1
           with:
             images: DinDockerRepo/AppNamn
             tags: |
               latest
               ${{ steps.meta.outputs.tags }}
         
         - name: Build and push Docker image
           uses: docker/build-push-action@v6.7.0
           with:
             context: .
             push: true
             tags: ${{ steps.meta.outputs.tags }}
             labels: ${{ steps.meta.outputs.labels }}
   ```

7. Gå till toppen av sidan.
8. I sektionen **"Commit changes"**, skriv en commit-meddelande, t.ex. "Lägger till GitHub Actions workflow".
9. Välj **"Commit directly to the main branch"**.
10. Klicka på **"Commit changes"**.

### Steg 6: Bekräfta att Workflowen är skapad
1. Gå till fliken **"Actions"** i ditt repository.
2. Du bör se din workflow listad där. Klicka på den för att se dess status och historik.

### Steg 7: Testa Workflowen
1. Gör en commit till `main`-grenen för att trigga workflowen.
2. Gå till **"Actions"**-fliken i ditt GitHub-repository för att se workflowens status.

### Steg 8: Skapa en CosmosDB för MongoDB (RU) på Azure

1. **Logga in på Azure Portal**
   - Gå till [Azure Portal](https://portal.azure.com/).

2. **Skapa en ny resurs**
   - Klicka på **"Create a resource"** i vänstermenyn.

3. **Sök efter Azure Cosmos DB**
   - Skriv **"Azure Cosmos DB"** i sökfältet och välj det från resultaten.

4. **Välj "Cosmos DB for MongoDB (RU)"**
   - Klicka på **"Create"**.
   - Välj **"Cosmos DB for MongoDB (RU)"** under API-alternativen.

5. **Fyll i nödvändig information**
   - **Subscription**: Välj din prenumeration.
   - **Resource Group**: Välj en befintlig resursgrupp eller skapa en ny för att organisera din Cosmos DB-instans.
   - **Account Name**: Ange ett unikt namn för din Cosmos DB. Observera att detta namn måste vara globalt unikt på Azure.
   - **Location**: Välj en region nära dig för bättre prestanda.

6. **Ange genomströmningskapacitet (RU/s)**
   - Under **"Capacity mode"**, välj **"Provisioned throughput"** och ange det önskade antalet RU/s (Request Units) i fältet **"Throughput"**. 

7. **Konfigurera ytterligare inställningar (valfritt)**
   - **Networking**: Välj om du vill tillåta offentlig åtkomst och ange eventuella nätverksregler.
   - **Backup**: Konfigurera backup-inställningar om det behövs.
   - **Tags**: Lägg till taggar för resursgruppering och hantering.

8. **Granska och skapa**
   - Klicka på **"Review + create"**.
   - Granska inställningarna noggrant.
   - Klicka på **"Create"** för att skapa CosmosDB-instansen.

9. **Bekräfta skapandet**
   - Efter några minuter bör du se ett meddelande om att din Cosmos DB-instans har skapats. Du kan gå till resursen direkt från meddelandet eller via din resurslista.

10. **Hämta anslutningssträng**
    - När din Cosmos DB är skapad, gå till instansen.
    - Under **"Settings"**, välj **"Connection String"** för att hämta anslutningssträngen som behövs för att ansluta till din databas från din applikation.



### Steg 9: Provisionera VM med `vm_provision.sh`

1. **Öppna en terminal** på din lokala maskin eller på Azure Cloud Shell.

2. **Navigera till provisioning-mappen** där `vm_provision.sh` och `cloud-init_docker.yaml` finns.

3. **Innan du kör skriptet**, se till att lägga till anslutningssträngen för CosmosDB och uppdatera `image` i `docker-compose.yml`-filen i `cloud-init_docker.yaml`. Här är en översikt av hur du ska göra:

   **Uppdatera `cloud-init_docker.yaml`:**
   ```yaml
   #cloud-config

   runcmd:
     ...
     - |
       cat <<EOF > /home/azureuser/docker-compose.yml
       services:
         app:
           image: <Byt ut mot din Docker-image>
           restart: always
           ports:
             - "8080:8080"
           environment:
             - MongoDbSettings__ConnectionString=<Byt ut mot din connectio-string>
             - TODO_SERVICE_IMPLEMENTATION=MongoDb
             - ASPNETCORE_ENVIRONMENT=Development
       EOF
     ...
   ```

4. **Kör skriptet för att skapa resursgruppen och VM:**
   ```bash
   ./vm_provision.sh
   ```

### Steg 10: Bekräfta att VM:en är skapad och miljön är korrekt uppbyggd

1. **Gå tillbaka till Azure Portal.**
2. **Navigera till din resursgrupp** och verifiera att VM:en är listad där.
3. **Öppna webbläsaren** och navigera till `http://<VM-IP-adress>:8080` för att komma åt appen.
4. **Testa appen** genom att göra några inmatningar och spara data.
5. **Gå tillbaka till Azure Portal** och navigera till din Cosmos DB-instans.
6. **Öppna Data Explorer**.
7. **Verifiera att den data som matas in finns** i Cosmos DB genom att bläddra i de insamlade dokumenten.
8. **Om den inmatade datan finns på plats**, bekräftar detta att:
   - VM:en har skapats korrekt.
   - Docker och Docker Compose har installerats och konfigurerats ordentligt.
   - Appen körs som förväntat på port 8080.
   - Anslutningen mellan appen och Cosmos DB fungerar utan problem.

9. **Om allt är korrekt** kan du nu börja använda applikationen i produktionsmiljö eller göra ytterligare tester och förbättringar.

# Avslutning

Denna guide har genererats med hjälp av ChatGPT, där alla prompts noggrant har utformats för att säkerställa att processen fungerar som avsett. Genom att följa stegen i denna guide har du nu en fullt fungerande Docker-baserad ToDo-applikation som körs på en Azure VM med Cosmos DB som databas. Om du har några frågor eller behöver ytterligare hjälp är du välkommen att utforska dokumentationen för de verktyg och tjänster som används, eller ställa frågor i relevanta forum. Tack för att du följde guiden, och lycka till med dina framtida projekt!

