📌 Event Todo Assistant — Console App (C# / .NET)

## Project Brief ##
A lightweight, structured, productivity‑focused console application for managing tasks, events, and AI‑generated suggestions.
Built in C# / .NET, designed for clarity, modularity, and future AI integration (Cloud AI or Local LLM via Ollama).

🚀 Features (Current Stage)

✅ Task Management

Add tasks with:

Title

Project

Priority

Description

Tags

Due date

Edit tasks

Delete tasks

Mark tasks as completed

View all tasks in a clean formatted list

🔍 Search & Filter System
Search by project

Filter by date (before / after / on)

Filter by status (completed / not completed)

Search by tags

Clean, readable result formatting

🤖 AI Suggestions (Mock AI for now)
Generate event‑based suggestions using a mock AI engine

Save suggestions as tasks

Delete suggestions

Save all suggestions at once

Architecture prepared for:

Cloud AI (OpenAI / Azure OpenAI / Claude / Groq)

Local AI (Ollama, LM Studio, GPT4All)

💾 Data Persistence
Tasks saved to tasks.json

Auto‑load on startup

Auto‑save on exit

🧱 Clean Architecture
Models → TaskItem, AISuggestionItem

Services → TaskManager, AISuggestionService, IAIService, Adapter

UI → MenuUI, DisplayService

Program.cs → Main loop + dependency setup

🧩 Project Structure
```text
EventTodoAssistant/
│
├── Models/
│   ├── TaskItem.cs
│   ├── AISuggestionItem.cs
│
├── Services/
│   ├── TaskManager.cs
│   ├── AISuggestionService.cs
│   ├── AISuggestionServiceAdapter.cs
│   ├── IAIService.cs
│   ├── CloudAIService.cs        (future)
│   ├── LocalAIService.cs        (future)
│
├── UI/
│   ├── MenuUI.cs
│   ├── DisplayService.cs
│
├── tasks.json
└── Program.cs
```

🧠 AI Integration Roadmap
The project is already structured to support multiple AI providers:

🔹 1. Mock AI (current)
Used for development and offline testing.

🔹 2. Cloud AI (planned)
Planned support for:

OpenAI API

Azure OpenAI

Claude

Groq

Mistral

🔹 3. Local AI (planned)
Planned support for:

Ollama (http://localhost:11434/api/generate)

LM Studio

GPT4All

llama.cpp

The IAIService interface allows switching AI providers with one line of code.

🛠️ How to Run
1. Clone the repository
git clone https://github.com/<your-username>/<your-repo>.git

2. Navigate into the project
cd EventTodoAssistant

3. Run the app at the temrinal of VS code.
Be sure you are in the right folder. Then: 

dotnet
dotnet run

📦 Requirements
.NET 6 or later

Windows / macOS / Linux

(Optional) Ollama installed for local LLM

(Optional) API key for cloud AI

🌱 Future Enhancements
AI‑generated event timelines

Natural‑language task creation

Priority scoring using AI

Export tasks to calendar

Import tasks from text or voice

Color‑coded console UI

Combined filters (project + date + status)

Statistics dashboard

Thanks for Stopping by !