import logging
from typing import Dict, Optional
from fastapi import APIRouter, Request, Depends, HTTPException, Query
from fastapi.responses import HTMLResponse
from sqlalchemy.orm import Session
from ...dependencies import get_db
from ...services.calculator_service import CalculatorService
from ...core.templates import templates

logger = logging.getLogger(__name__)
router = APIRouter()

@router.get("/resultados/{perfil}", response_class=HTMLResponse)
def exibir_resultados_perfil(
    perfil: str,
    request: Request,
    db: Session = Depends(get_db),
    # Captura todos os parâmetros da query como dict de float
    params: Dict[str, float] = Depends(lambda request: {
        k: float(v) for k, v in request.query_params.items()
        if v.replace('.', '', 1).isdigit()
    })
):
    service = CalculatorService(db)
    try:
        resultados = service.calculate_for_perfil(perfil, params=params)
        melhor_codigo = service.selecionar_longarina_valida()
        logger.info(f"[DEBUG] Melhor longarina selecionada no endpoint: {melhor_codigo}")
        if melhor_codigo is None:
            raise ValueError("Nenhuma longarina válida encontrada")
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))

    return templates.TemplateResponse(
        "resultados.html",
        {
            "request": request,
            "perfil": perfil,
            "resultados": resultados,
            "melhor_codigo": melhor_codigo,
        },
    )


