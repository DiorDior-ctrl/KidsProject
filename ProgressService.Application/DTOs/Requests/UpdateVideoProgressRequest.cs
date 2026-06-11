
namespace ProgressService.Application.DTOs.Requests;

public record UpdateVideoProgressRequest(
    int CurrentSeconds,
    int TotalSeconds
);