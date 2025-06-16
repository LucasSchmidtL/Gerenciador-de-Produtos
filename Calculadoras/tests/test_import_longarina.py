class CalculatorService:
    # Simulando dados, pode trocar pela chamada real ao banco depois
    longarinas_teste = [
        {"perfil": "L100", "situacao": "Verdadeira", "PesoLinear": 10.5},
        {"perfil": "L123", "situacao": "Falso", "PesoLinear": 9.8},
        {"perfil": "L200", "situacao": "Verdadeira", "PesoLinear": 9.0},
    ]

    def selecionar_longarina_valida(self, longarinas):
        validas = [l for l in longarinas if l["situacao"] == "Verdadeira"]
        if not validas:
            print("Nenhuma longarina válida encontrada")
            return None
        menor_peso = min(l["PesoLinear"] for l in validas)
        selecionada = next(l for l in validas if l["PesoLinear"] == menor_peso)
        print(f"Longarina selecionada: {selecionada['perfil']}, peso: {selecionada['PesoLinear']}")
        return selecionada["perfil"]

# Criando instância e rodando teste
service = CalculatorService()
perfil = service.selecionar_longarina_valida(service.longarinas_teste)
print(f"Perfil selecionado no teste: {perfil}")
