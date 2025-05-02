# DClare Runtime

**DClare Runtime** is the official, open-source, and modular runtime for executing agents defined using the [DClare specification](https://github.com/d-clare/specification).

[![License](https://img.shields.io/github/license/d-clare/runtime)](LICENSE)
[![Build Status](https://img.shields.io/github/actions/workflow/status/d-clare/runtime/test.yml?branch=main)](https://github.com/d-clare/runtime/actions)
[![Release](https://img.shields.io/github/v/release/d-clare/runtime?include_prereleases)](https://github.com/d-clare/runtime/releases)
[![NuGet](https://img.shields.io/nuget/v/DClare.Runtime.Integration.svg)](https://nuget.org/packages/DClare.Runtime.Integration)
[<img src="http://img.shields.io/badge/Website-blue?style=flat&logo=google-chrome&logoColor=white">](https://d-clare.ai/) 
[<img src="https://img.shields.io/badge/LinkedIn-blue?logo=linkedin&logoColor=white">](https://www.linkedin.com/company/d-clare/)

---

## ✨ Features

- ⚙️ **Declarative configuration** of agents and related resources
- 🤝 **Pluggable communication protocols** (e.g. [A2A](https://github.com/google/A2A))
- 🧠 **Hosted and remote agent execution** via kernel-driven logic
- 📦 Designed for integration in ASP.NET Core and other .NET environments

---

## 📂 Structure

The runtime interprets a declarative component manifest (in YAML or JSON), defining:

- **Kernels** — underlying reasoning engines (e.g. Azure OpenAI, OpenAI)
- **Authentications** — credentials and authorization details used to access external services
- **Toolsets** — collections of tools that agents can invoke at runtime
- **Memories** — external or local memory providers used to persist or retrieve contextual data
- **Agents** — hosted or remote capabilities, built atop kernels

---

## 🛠️ Runtime Component Example

Below is an example structure used by the runtime to define an application:

```yaml
components:
  kernels:
    openai:
      reasoning:
        provider: openai
        model: gpt-4o
  agents:
    openai:
      hosted:
        description: A general-purpose assistant capable of performing a wide range of reasoning, generation, and analysis tasks
        instructions: >
          You are an advanced AI assistant.

          Your purpose is to provide high-quality, context-aware responses to a wide variety of prompts. You may be asked to:
          - Answer questions accurately and clearly
          - Generate or edit structured and unstructured text
          - Analyze input for correctness, clarity, or quality
          - Help decompose or synthesize information
          - Assist with reasoning, summarization, or evaluation
          
          Always respond with clarity, precision, and relevance. If the prompt is ambiguous or lacks sufficient context, respond by asking for clarification.
          
          You are expected to behave in a neutral, helpful, and objective manner at all times.
        kernel:
          use: openai
```

---

## 📖 Documentation

Comprehensive documentation is available in the [DClare specification repository](https://github.com/d-clare/specification), including:

- Component schemas
- Runtime behavior
- Integration guides
- Protocol support (e.g. [A2A](https://github.com/google/A2A))

---

## 🧑‍💻 Contributing

We welcome contributions! See [CONTRIBUTING.md](CONTRIBUTING.md) for instructions on how to get started.

---

## 📫 Contact

For questions, suggestions, or collaboration:
**📧 contact@d-clare.ai**

---

## 🪪 License

Licensed under the [Apache License 2.0](LICENSE).
