# calculadoras/core/templates.py
import os
from random import random                 # ← 1) importar
from fastapi.templating import Jinja2Templates

BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
TEMPLATES_DIR = os.path.join(BASE_DIR, "templates")

templates = Jinja2Templates(directory=TEMPLATES_DIR)

# ────────────────────────────────────────────────────────────
#  registra o helper random() para ser usado nos templates
# ────────────────────────────────────────────────────────────
templates.env.globals["random"] = random   # ← 2) registrar
