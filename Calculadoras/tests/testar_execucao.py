from ..legacy.main import executar

entrada = {
    "AlturaLongarina": 100.0,
    "LarguraLongarina": 80.0,
    "CoordenadaHorizontalCentroCorteRelativoCentroide": 30.0,
    "CoordenadaVerticalCentroCorteRelativoCentroide": 40.0,
    # Parâmetros que a equação requer:
    "rx": 5.0,
    "ry": 6.0,
    "x0": 2.0,
    "y0": 3.0,
}

resultado = executar(
    "calcular_raio_giracao_polar_bruta_em_relacao_eixo_torcao",
    entrada
)
print(resultado)
