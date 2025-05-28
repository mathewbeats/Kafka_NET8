# 🏭 Lendit Kafka Product-User Event System

Este proyecto demuestra el uso de **Apache Kafka** para manejar eventos de **productos** y **usuarios**, con **Entity Framework Core** y una base de datos **SQLite** para almacenamiento persistente. Se incluye el uso de **Docker Compose** para levantar el entorno de Kafka y un navegador gráfico de SQLite para verificar los datos.

---

## 📑 Tabla de Contenido
- [🚀 Requisitos Previos](#requisitos-previos)
- [📦 Estructura del Proyecto](#estructura-del-proyecto)
- [⚙️ Configuración y Levantamiento de Docker](#configuración-y-levantamiento-de-docker)
- [🔗 Flujo General del Sistema](#flujo-general-del-sistema)
- [💾 Base de Datos con EF Core y SQLite](#base-de-datos-con-ef-core-y-sqlite)
- [🖥️ Ver Datos con DB Browser for SQLite](#ver-datos-con-db-browser-for-sqlite)
- [🧑‍💻 Próximos Pasos](#próximos-pasos)

---

## 🚀 Requisitos Previos
- **.NET SDK 9.0**
- **Docker** y **Docker Compose**
- **Kafka y Zookeeper** (configurados vía Docker Compose)
- **DB Browser for SQLite** (opcional para inspección gráfica)

---

## 📦 Estructura del Proyecto
📂 Lendit
- ├── 📁 Data # Proyecto para Entity Framework Core y DbContext
- ├── 📁 KafkaProducer # Produce eventos de productos y usuarios a Kafka
- ├── 📁 KafkaConsumer # Consume eventos de productos y usuarios desde Kafka
- ├── 📁 ModelsLibrary # Clases de dominio, entidades y modelos compartidos
- ├── docker-compose.yml # Configuración para Kafka y Zookeeper


---

## ⚙️ Configuración y Levantamiento de Docker
```bash
cd /Users/alejandromateo/RiderProjects/Lendit
docker compose up -d
docker ps   # Para verificar
# (Opcional) Detener y limpiar
docker compose down

## Diagrama de Arquitectura
graph TD
    A[KafkaProducer] -- Produce ProductEvent/UserEvent --> B[Kafka Topic: products-topic]
    A -- Produce UserEvent --> C[Kafka Topic: users-topic]
    B --> D[KafkaConsumer]
    C --> E[KafkaUserConsumer]
    D -- Guarda Productos --> F[(SQLite: Products)]
    E -- Guarda Usuarios --> G[(SQLite: Users)]
    subgraph Kafka Cluster
        B
        C
    end
    subgraph Database
        F
        G
    end
---

## 📝 Descripción
- KafkaProducer produce eventos ProductEvent y UserEvent a sus respectivos topics.
- KafkaConsumer y (futuro) KafkaUserConsumer consumen mensajes de products-topic y users-topic.
- Los datos se guardan en la base de datos local SQLite en tablas Products y Users.
- Docker levanta Zookeeper, Kafka y (opcionalmente) Kafka UI para monitoreo.

---

## Base de Datos con EF Core y SQLite

- 1️⃣ DbContext configurado para SQLite:
-- optionsBuilder.UseSqlite("Data Source=Lendit.db");
-- brew install db-browser-for-sqlite
-- open Lendit/KafkaProducer/Lendit.db


