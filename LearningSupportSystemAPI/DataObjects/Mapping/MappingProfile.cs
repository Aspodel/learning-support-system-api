using AutoMapper;

namespace LearningSupportSystemAPI;

public class MappingProfile : Profile
{
    #region [Ctor]
    public MappingProfile()
    {
        CreateMap<Role, RoleDTO>();
        CreateMap<RoleDTO, Role>()
            .ForMember(d => d.Id, opt => opt.Ignore());


        CreateMap<User, UserDTO>()
            .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.UserRoles!.Select(ur => ur.Role!.Name)));
        CreateMap<UserDTO, User>()
            .ForMember(d => d.IdCard, opt => opt.Ignore());
        CreateMap<CreateUserDTO, User>();

        CreateMap<Administrator, AdminDTO>();
        CreateMap<AdminDTO, Administrator>()
            .ForMember(d => d.IdCard, opt => opt.Ignore());
        CreateMap<CreateAdminDTO, Administrator>();

        CreateMap<Student, StudentDTO>()
            .ForMember(d => d.RegisteredClasses, opt => opt.MapFrom(s => s.RegisteredClasses.Select(c => c.ClassId)));
        CreateMap<StudentDTO, Student>()
            .ForMember(d => d.IdCard, opt => opt.Ignore())
            .ForMember(d => d.DepartmentId, opt => opt.Ignore())
            .ForMember(d => d.RegisteredClasses, opt => opt.Ignore());
        CreateMap<CreateStudentDTO, Student>();
        CreateMap<CreateStudentExcelDTO, Student>()
            .ForMember(s => s.Department, opt => opt.Ignore());
        CreateMap<Student, ExportStudentDTO>()
            .ForMember(d => d.DateOfBirth, opt => opt.MapFrom(s => s.DateOfBirth.ToString("dd/MM/yyyy")));


        CreateMap<Lecturer, LecturerDTO>()
            .ForMember(d => d.Classes, opt => opt.Ignore());
        // .ForMember(d => d.Classes, opt => opt.MapFrom(s => s.Classes!.Select(c => c.Id)));
        CreateMap<LecturerDTO, Lecturer>()
            .ForMember(d => d.IdCard, opt => opt.Ignore())
            .ForMember(d => d.DepartmentId, opt => opt.Ignore())
            .ForMember(d => d.Classes, opt => opt.Ignore());
        CreateMap<CreateLecturerDTO, Lecturer>();
        CreateMap<CreateLecturerExcelDTO, Lecturer>()
            .ForMember(s => s.Department, opt => opt.Ignore());


        CreateMap<Announcement, AnnouncementDTO>();
        CreateMap<AnnouncementDTO, Announcement>()
            .ForMember(d => d.Id, opt => opt.Ignore());


        CreateMap<Class, ClassDTO>();
        CreateMap<ClassDTO, Class>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.GradeColumns, opt => opt.Ignore())
            .ForMember(d => d.Students, opt => opt.Ignore());
        CreateMap<CreateClassDTO, Class>();
        CreateMap<CreateClassExcelDTO, Class>();


        CreateMap<Course, CourseDTO>();
        CreateMap<CourseDTO, Course>()
            .ForMember(d => d.Id, opt => opt.Ignore());
        CreateMap<CreateCourseDTO, Course>()
            .ForMember(d => d.PrerequisiteFor, opt => opt.Ignore())
            .ForMember(d => d.Prerequisites, opt => opt.Ignore());



        CreateMap<CoursePrerequisite, CoursePrerequisiteDTO>();
        CreateMap<CoursePrerequisiteDTO, CoursePrerequisite>();


        CreateMap<Department, DepartmentDTO>();
        CreateMap<DepartmentDTO, Department>()
            .ForMember(d => d.Id, opt => opt.Ignore());
        CreateMap<CreateDepartmentDTO, Department>()
            .ForMember(d => d.FacultyOfficeId, opt => opt.Ignore());


        CreateMap<Discussion, DiscussionDTO>();
        CreateMap<DiscussionDTO, Discussion>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Type, opt => opt.Ignore());
        CreateMap<CreateDiscussionDTO, Discussion>();


        CreateMap<GradeColumn, GradeColumnDTO>();
        CreateMap<GradeColumnDTO, GradeColumn>();
        // .ForMember(d => d.Id, opt => opt.Ignore());


        CreateMap<Grade, GradeDTO>();
        CreateMap<GradeDTO, Grade>();
        // .ForMember(d => d.Id, opt => opt.Ignore());


        CreateMap<Group, GroupDTO>();
        CreateMap<GroupDTO, Group>()
            .ForMember(d => d.Id, opt => opt.Ignore());
        CreateMap<CreateGroupDTO, Group>()
            .ForMember(d => d.Students, opt => opt.Ignore());


        CreateMap<Major, MajorDTO>();
        CreateMap<MajorDTO, Major>()
            .ForMember(d => d.Id, opt => opt.Ignore());
        CreateMap<CreateMajorDTO, Major>()
            .ForMember(d => d.DepartmentId, opt => opt.Ignore());


        CreateMap<Message, MessageDTO>();
        CreateMap<MessageDTO, Message>()
            .ForMember(d => d.Id, opt => opt.Ignore());


        CreateMap<Notification, NotificationDTO>();
        CreateMap<NotificationDTO, Notification>()
            .ForMember(d => d.Id, opt => opt.Ignore());


        CreateMap<Room, RoomDTO>();
        CreateMap<RoomDTO, Room>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Department, opt => opt.Ignore());
        CreateMap<CreateRoomDTO, Room>();


        CreateMap<Semester, SemesterDTO>();
        CreateMap<SemesterDTO, Semester>()
            .ForMember(d => d.Id, opt => opt.Ignore());


        CreateMap<StudentClass, StudentClassDTO>();
        CreateMap<StudentClassDTO, StudentClass>();

        CreateMap<StudentTask, StudentTaskDTO>();
        CreateMap<StudentTaskDTO, StudentTask>();

        CreateMap<ToDoItem, ToDoItemDTO>();
        CreateMap<ToDoItemDTO, ToDoItem>()
            .ForMember(d => d.Id, opt => opt.Ignore());
        // .ForMember(d => d.Students, opt => opt.Ignore());
        CreateMap<CreateToDoItemDTO, ToDoItem>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Students, opt => opt.Ignore());

        CreateMap<ToDoList, ToDoListDTO>();
        CreateMap<ToDoListDTO, ToDoList>()
            .ForMember(d => d.Id, opt => opt.Ignore());
        CreateMap<CreateToDoListDTO, ToDoList>();

        CreateMap<Assignment, AssignmentDTO>();
        CreateMap<AssignmentDTO, Assignment>();
        CreateMap<CreateAssignmentDTO, Assignment>();

        CreateMap<Question, QuestionDTO>();
        CreateMap<QuestionDTO, Question>();

        CreateMap<Answer, AnswerDTO>();
        CreateMap<AnswerDTO, Answer>();

        CreateMap<Submission, SubmissionDTO>();
        CreateMap<SubmissionDTO, Submission>();

        CreateMap<FileSubmission, FileSubmissionDTO>();
        CreateMap<FileSubmissionDTO, FileSubmission>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore());
        CreateMap<CreateFileSubmissionDTO, FileSubmission>();

        CreateMap<AnswerSubmission, AnswerSubmissionDTO>();
        CreateMap<AnswerSubmissionDTO, AnswerSubmission>();
    }
    #endregion
}
