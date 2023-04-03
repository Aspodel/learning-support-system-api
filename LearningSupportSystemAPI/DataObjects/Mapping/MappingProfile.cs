using AutoMapper;
using LearningSupportSystemAPI.Core.Entities;
using LearningSupportSystemAPI.Core.Entities.JoinTables;

namespace LearningSupportSystemAPI.DataObjects.Mapping
{
    public class MappingProfile : Profile
    {
        #region [Ctor]
        public MappingProfile()
        {
            CreateMap<Role, RoleDTO>();
            CreateMap<RoleDTO, Role>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<User, UserDTO>()
                .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.UserRoles.Select(ur => ur.Role!.Name)));
            CreateMap<UserDTO, User>();
                //.ForMember(d => d.Guid, opt => opt.Ignore());
            CreateMap<CreateUserDTO, User>();


            CreateMap<Announcement, AnnouncementDTO>();
            CreateMap<AnnouncementDTO, Announcement>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<Class, ClassDTO>();
            CreateMap<ClassDTO, Class>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<Course, CourseDTO>();
            CreateMap<CourseDTO, Course>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<CoursePrerequisite, CoursePrerequisiteDTO>();
            CreateMap<CoursePrerequisiteDTO, CoursePrerequisite>();


            CreateMap<Department, DepartmentDTO>();
            CreateMap<DepartmentDTO, Department>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<Discussion, DiscussionDTO>();
            CreateMap<DiscussionDTO, Discussion>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<GradeColumn, GradeColumnDTO>();
            CreateMap<GradeColumnDTO, GradeColumn>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<Grade, GradeDTO>();
            CreateMap<GradeDTO, Grade>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<Group, GroupDTO>();
            CreateMap<GroupDTO, Group>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<Major, MajorDTO>();
            CreateMap<MajorDTO, Major>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<Message, MessageDTO>();
            CreateMap<MessageDTO, Message>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<Notification, NotificationDTO>();
            CreateMap<NotificationDTO, Notification>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<Room, RoomDTO>();
            CreateMap<RoomDTO, Room>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<Semester, SemesterDTO>();
            CreateMap<SemesterDTO, Semester>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<StudentClass, StudentClassDTO>();
            CreateMap<StudentClassDTO, StudentClass>();
        }
        #endregion
    }
}
