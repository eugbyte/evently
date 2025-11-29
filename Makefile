dev:
	dotnet run --project=src/Evently.Server/Evently.Server.csproj --launch-profile https

test:
	dotnet test ./tests/Evently.Server.Test/Evently.Server.Test.csproj
	
add-migration:
	dotnet ef migrations add UpdateSeededDates --project=src/Evently.Server --context=AppDbContext --output-dir=Common/Adapters/Data/Migrations

update-migration:
	dotnet ef database update --project=src/Evently.Server --context=AppDbContext

remove-migration:
	dotnet ef migrations remove --project=src/Evently.Server --context=AppDbContext
	
fmt:
	dotnet tool restore
	cd src/Evently.Server && dotnet csharpier format .
	cd src/evently.client && npm run fmt
    
docker-build-no-cache:
	docker build --no-cache --progress=plain --tag eugbyte/evently:latest -f src/Evently.Server/Dockerfile .

docker-build:
	docker build --tag eugbyte/evently:latest -f src/Evently.Server/Dockerfile .

docker-clean:
	docker system prune --all --volumes --force

tf-build:
	terraform plan -var-file="dev.tfvars"
	terraform apply -var-file="dev.tfvars" -auto-approve
	
clean-tf:
	rm -rf ./deploy/Terraform/.terraform
	rm ./deploy/Terraform/.terraform.lock.hcl

debug-container-app:
	az containerapp logs show --name ca-evently-prod-sea --resource-group rg-evently-dev-sea --follow