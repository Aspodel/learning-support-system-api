using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;

[Route("api/[controller]")]
[ApiController]
public class SubmissionController : ControllerBase
{
    #region [Fields]
    private readonly IFileSubmissionRepository _fileSubmissionRepository;
    private readonly IAnswerSubmissionRepository _answerSubmissionRepository;
    private readonly IBlobService _blobService;
    private readonly IMapper _mapper;
    #endregion

    #region [Ctor]
    public SubmissionController(IFileSubmissionRepository fileSubmissionRepository, IAnswerSubmissionRepository answerSubmissionRepository, IBlobService blobService, IMapper mapper)
    {
        _fileSubmissionRepository = fileSubmissionRepository;
        _answerSubmissionRepository = answerSubmissionRepository;
        _blobService = blobService;
        _mapper = mapper;
    }
    #endregion

    #region [GET]
    // [HttpGet]
    // public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    // {
    //     var assignments = await _submissionRepository.FindAll().ToListAsync(cancellationToken);
    //     return Ok(_mapper.Map<IEnumerable<AssignmentDTO>>(assignments));
    // }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> Get(int id)
    // {
    //     var assignment = await _submissionRepository.FindByIdAsync(id);
    //     if (assignment is null)
    //         return NotFound();

    //     return Ok(_mapper.Map<AssignmentDTO>(assignment));
    // }

    [HttpGet("file/group/{groupId}")]
    public async Task<IActionResult> GetFileByGroup(int groupId, CancellationToken cancellationToken = default)
    {
        var submissions = await _fileSubmissionRepository
               .FindAll(f => f.Student.RegisteredClasses.Any(rc => rc.GroupId == groupId))
               .ToListAsync(cancellationToken);

        return Ok(_mapper.Map<IEnumerable<FileSubmissionDTO>>(submissions));
    }

    [HttpGet("file/class/{classId}")]
    public async Task<IActionResult> GetFileByClass(int classId, CancellationToken cancellationToken = default)
    {
        var submissions = await _fileSubmissionRepository.FindAll(f => f.Assignment!.ClassId == classId).ToListAsync(cancellationToken);
        return Ok(_mapper.Map<IEnumerable<FileSubmissionDTO>>(submissions));
    }
    #endregion

    #region [POST]
    [HttpPost("file")]
    public async Task<IActionResult> CreateFileSubmission([FromForm] CreateFileSubmissionDTO dto, CancellationToken cancellationToken = default)
    {
        var fileSubmission = _mapper.Map<FileSubmission>(dto);

        var fileName = $"{fileSubmission.AssignmentId}/{fileSubmission.FileName}_{fileSubmission.CreatedAt:yyyyMMddHHmmss}";
        var fileExtension = Path.GetExtension(dto.File.FileName);

        fileSubmission.FileUrl = await _blobService.UploadFileAsync(dto.File, fileName);

        fileSubmission.FileName = $"{fileSubmission.FileName}_{fileSubmission.CreatedAt:yyyyMMddHHmmss}{fileExtension}";
        _fileSubmissionRepository.Add(fileSubmission);
        await _fileSubmissionRepository.SaveChangesAsync(cancellationToken);

        return Ok(_mapper.Map<FileSubmissionDTO>(fileSubmission));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAnswerSubmission([FromBody] AnswerSubmissionDTO dto, CancellationToken cancellationToken = default)
    {
        var answerSubmission = _mapper.Map<AnswerSubmission>(dto);
        _answerSubmissionRepository.Add(answerSubmission);
        await _answerSubmissionRepository.SaveChangesAsync(cancellationToken);

        return Ok(_mapper.Map<AnswerSubmissionDTO>(answerSubmission));
    }
    #endregion

    #region [PUT]
    // [HttpPut]
    // public async Task<IActionResult> Update([FromBody] AssignmentDTO dto, CancellationToken cancellationToken = default)
    // {
    //     // var assignment = await _submissionRepository.FindByIdAsync(dto.Id, cancellationToken);
    //     // if (assignment is null)
    //     //     return NotFound();

    //     // _mapper.Map(dto, assignment);
    //     // _submissionRepository.Update(assignment);
    //     // await _submissionRepository.SaveChangesAsync(cancellationToken);

    //     return NoContent();
    // }
    #endregion

    #region [DELETE]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        // var assignment = await _submissionRepository.FindByIdAsync(id, cancellationToken);
        // if (assignment is null)
        //     return NotFound();

        // _submissionRepository.Delete(assignment);
        // await _submissionRepository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
    #endregion
}
