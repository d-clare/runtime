# DClare Runtime

**DClare Runtime** is the official, open-source, and modular runtime for executing agentic workflows defined using the [DClare specification](https://github.com/d-clare/specification).

[![Build Status](https://img.shields.io/github/actions/workflow/status/d-clare/runtime/test.yml?branch=main)](https://github.com/d-clare/runtime/actions)
[![Release](https://img.shields.io/github/v/release/d-clare/runtime?include_prereleases)](https://github.com/d-clare/runtime/releases)
[![NuGet](https://img.shields.io/nuget/v/DClare.Runtime.Api.svg)](https://nuget.org/packages/DClare.Runtime.Api)
[![License](https://img.shields.io/github/license/d-clare/runtime)](LICENSE)
[<img src="http://img.shields.io/badge/Website-blue?style=flat&logo=google-chrome&logoColor=white">](https://d-clare.ai/) 
[<img src="https://img.shields.io/badge/LinkedIn-blue?logo=linkedin&logoColor=white">](https://www.linkedin.com/company/d-clare/)

---

## ✨ Features

- ⚙️ **Declarative configuration** of agents, kernels, and processes
- 🤝 **Pluggable communication protocols** (e.g. [A2A](https://github.com/google/A2A))
- 🧠 **Hosted and remote agent execution** via kernel-driven logic
- 🔁 **Process convergence and synthesis logic** for multi-agent workflows
- 📦 Designed for integration in ASP.NET Core and other .NET environments

---

## 📂 Structure

The runtime interprets a declarative component manifest (in YAML or JSON), defining:

- **Kernels** — underlying reasoning engines (e.g. Azure OpenAI, OpenAI)
- **KernelFunctions** — reusable prompt/function templates executable by kernels
- **Authentications** — credentials and authorization details used to access external services
- **Toolsets** — collections of tools that agents can invoke at runtime
- **Memories** — external or local memory providers used to persist or retrieve contextual data
- **Agents** — hosted or remote capabilities, built atop kernels
- **Processes** — orchestration of agents through convergence/synthesis strategies

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
  processes:
    reviewQuestion:
      convergence:
        strategy:
          decomposition:
            promptVariableName: prompt
            agentsVariableName: agents
            kernel:
              use: openai
            function:
              template:
                content: |
                  You are a task planner responsible for distributing a complex instruction to multiple specialized agents.

                  Your task:
                  1. Carefully analyze the user's original prompt.
                  2. Determine how each agent listed below (based on name and description) can contribute meaningfully.
                  3. For each agent, generate a standalone subprompt tailored to their specific function.
                  4. Each subprompt must:
                     - Be fully self-contained.
                     - Include all necessary context, including a restatement of the original user prompt.
                     - Be phrased as a direct and actionable instruction for that specific agent.
                     - Not assume the agent has access to any external context.
                  
                  ---
                  
                  **Original User Prompt:**
                  {{ $prompt }}
                  
                  ---
                  
                  **Available Agents:**
                  {{ $agents }}
                  
                  ---
                  
                  **Output Instructions:**
                  - Return only a raw, unformatted JSON object as your full output.
                    - Do NOT wrap the JSON in triple backticks (```).
                    - Do NOT add any explanation, comment, or formatting.
                    - Do NOT prepend or append any extra text.
                    - Your response MUST be a plain JSON object only — nothing else.
                  - Use the agent names as keys.
                  - Use the generated subprompts as values.
                  - Do **not** include any explanation, markdown, or formatting around the JSON.
                  
                  ---
                  
                  **Example Output:**
                  ```json
                  {
                    "Grammar": "Analyze the following prompt for grammar and spelling issues: What is the name of the King of Belgium?",
                    "Technical": "Evaluate the technical accuracy of the following networking question: What is the name of the King of Belgium?",
                    "Didactics": "Review the didactic quality of the following exam question: What is the name of the King of Belgium? Ensure it aligns with educational assessment standards."
                  }
          synthesis:
            responsesVariableName: responses
            kernel:
              use: openai
            function:
              template:
                content: |
                  You are an expert synthesizer responsible for combining multiple specialized insights into a single, cohesive response.

                  Your task:
                  1. Read the responses provided by various specialized agents.
                  2. Understand each response’s purpose and point of view based on the agent’s role.
                  3. Synthesize their content into a single, well-structured and informative answer.
                  4. Ensure that the final output is clear, non-redundant, and preserves all relevant contributions from each agent.
                  
                  ---
                  
                  **Agent Responses (as a JSON object):**
                  {{ $responses }}
                  
                  ---
                  
                  **Instructions:**
                  - Do not copy responses verbatim — blend them naturally into a single narrative.
                  - Do not refer to the agents by name.
                  - The final output should sound as if written by a single, knowledgeable expert.
        agents:
          GrammarAgent:
            hosted:
              description: Ensuring clarity, correctness, and readability through proper grammar, spelling, vocabulary, and concise phrasing
              instructions: |
                Your task is to refine text for clarity, correctness, and readability by ensuring proper grammar, spelling, vocabulary, and concise phrasing. Avoid ambiguity, redundant wording, and complex structures while maintaining the intended meaning and tone
              kernel:
                use: openai
          DidacticsAgent:
            hosted:
              description: Designing educational assessments that align with learning objectives, cognitive levels, and fair evaluation principles to ensure effective knowledge measurement
              instructions: |
                Your task is to design and refine educational assessments that align with learning objectives, cognitive levels, and fair evaluation principles. Ensure that each question effectively measures knowledge, maintains validity, and avoids bias while promoting clear and meaningful assessment.
              kernel:
                use: openai
          ExpertiseAgent:
            hosted:
              description: Ensuring content accuracy, relevance, and domain-specific validity by leveraging expert knowledge in the subject matter
              instructions: |
                Your task is to design and refine educational assessments that align with learning objectives, cognitive levels, and fair evaluation principles. Ensure that each question effectively measures knowledge, maintains validity, and avoids bias while promoting clear and meaningful assessment.
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
