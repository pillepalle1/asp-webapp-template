// Nuget + Cli
global using OneOf;
global using Dapper;
global using MediatR;
global using FluentValidation;

global using System.Data;
global using System.Data.Common;
global using System.Diagnostics;
global using System.Collections.Immutable;

global using Microsoft.Data.Sqlite;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Configuration;

// Application
global using AppTemplate.Application.Models;
global using AppTemplate.Application.Entities;
global using AppTemplate.Application.Managers;
global using AppTemplate.Application.Factories;
global using AppTemplate.Application.Extensions;
global using AppTemplate.Application.Repositories;
global using AppTemplate.Application.Persistence;
global using AppTemplate.Application.Cqrs.Common;