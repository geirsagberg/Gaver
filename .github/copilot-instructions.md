# Copilot Instructions for Gaver

## Project Overview

Gaver is a wish list application built with:
- **Backend**: ASP.NET Core (C#) with SignalR for real-time features
- **Frontend**: React with TypeScript, Material-UI, and Overmind for state management
- **Database**: Entity Framework Core

## Project Structure

```
/src
  /Gaver.Web        - Main web application (ASP.NET Core)
  /Gaver.Data       - Data access layer (Entity Framework)
  /Gaver.Common     - Shared contracts and utilities
  /SharpTypeGen     - TypeScript type generation tool
/frontend
  /src              - React application source
    /components     - Reusable UI components
    /pages          - Page-level components
    /overmind       - State management (actions, effects, state)
    /utils          - Utility functions
/test               - Test projects
```

## Tech Stack

### Backend
- .NET (version specified in global.json)
- ASP.NET Core with SignalR
- Entity Framework Core
- File-scoped namespaces (required)

### Frontend
- React 18 with TypeScript
- Material-UI (MUI) for UI components
- Overmind for state management
- Vite for bundling
- Bun as package manager

## Code Style & Conventions

### C# (.NET)
- **Use file-scoped namespaces** (enforced via .editorconfig)
- **Indent**: 4 spaces
- **Braces**: Same line (`csharp_new_line_before_open_brace = none`)
- **var**: Prefer `var` for built-in types and when type is apparent
- **Primary constructors**: Use when appropriate (see ListHub.cs example)
- **Expression-bodied members**: Use for single-line methods
- **Naming**: PascalCase for constants and public members
- No trailing whitespaces
- Use language keywords vs BCL types (`int` not `Int32`)

### TypeScript/React
- **Indent**: 2 spaces
- **Semicolons**: No semicolons (enforced by Prettier)
- **Quotes**: Single quotes
- **JSX**: Closing bracket on same line (`jsxBracketSameLine: true`)
- **Type safety**: Strict mode enabled
- **Path aliases**: Use `~/` for imports from `frontend/src`
- **React**: Use functional components with hooks
- **State management**: Use Overmind patterns (actions, effects, state)

### General
- UTF-8 encoding
- Trim trailing whitespace
- Insert final newline
- Max line length: 120 characters (except C#)

## Build & Development

### Start Development
```bash
bun start              # Start both frontend and backend
bun start:frontend     # Start Vite dev server only
bun start:backend      # Start .NET watch mode only
```

### Build
```bash
bun build              # Build both frontend and backend
bun build:frontend     # Build frontend (TypeScript + Vite)
bun build:backend      # Build .NET projects
```

### Testing
```bash
bun test               # Run all tests (frontend + backend)
bun test:frontend      # Run Jest tests
bun test:backend       # Run .NET tests
```

### Other Commands
```bash
bun clean              # Clean build artifacts
bun clean:www          # Clean wwwroot directory
```

## Key Patterns & Practices

### Backend
- Use dependency injection via primary constructors
- SignalR hubs in `/Hubs` directory
- Authorization via `[Authorize]` attribute
- Custom exceptions in `Gaver.Common.Exceptions`
- Access checking via `IAccessChecker`
- Mapping via `IMapperService`

### Frontend
- State management via Overmind (avoid local state when possible)
- API calls in Overmind effects
- Material-UI components for UI
- Type generation from C# models via SharpTypeGen
- SignalR for real-time updates

## Important Notes

1. **Package Manager**: This project uses **Bun**, not npm or yarn
2. **File-scoped namespaces**: All C# files must use file-scoped namespace syntax
3. **Code formatting**: Prettier and EditorConfig settings must be followed
4. **Real-time features**: The app uses SignalR for live updates
5. **Type generation**: TypeScript types are generated from C# models

## Testing Guidelines

- Frontend tests use Jest
- Backend tests use xUnit (located in `/test` directory)
- Test utilities are in `Gaver.TestUtils`

## When Making Changes

1. Follow existing code patterns in the codebase
2. Respect the `.editorconfig` and `.prettierrc` settings
3. Use file-scoped namespaces for all new C# files
4. Prefer functional components in React
5. Use Overmind for state management, not local state
6. Run tests after making changes
7. Build the project to verify no compilation errors
