using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using ContactBookWebApi.Entities;
using ContactBookWebApi.Mappings.DTOs.Contact;
using ContactBookWebApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactBookWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactBookService _contactBookService;
        private readonly IMapper _mapper;

        public ContactsController(IContactBookService contactBookService, IMapper mapper)
        {
            _contactBookService = contactBookService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactReadDto>>> GetAllContacts(string name = "")
        {
            try
            {
                IEnumerable<Contact> contactsFromService;

                if (name.Any(c => char.IsLetter(c)))
                {
                    contactsFromService = await _contactBookService.SearchContactsByName(name);
                }
                else
                {
                    string nameQueryWithoutSpaces = new string(name.Where(c => !char.IsWhiteSpace(c)).ToArray());
                    string modifiedQuery = Regex.Replace(nameQueryWithoutSpaces, "^([+])", "");

                    contactsFromService = await _contactBookService.SearchContactsByName(modifiedQuery);
                }

                IEnumerable<ContactReadDto> contactReadDtos = _mapper.Map<IEnumerable<ContactReadDto>>(contactsFromService);
                return Ok(contactReadDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("{id}", Name = "GetContactById")]
        public async Task<ActionResult<ContactReadDto>> GetContactById(int id)
        {
            try
            {
                Contact contactFromService = await _contactBookService.GetContactById(id);

                if (contactFromService == null)
                {
                    return NotFound();
                }

                ContactReadDto contactReadDto = _mapper.Map<ContactReadDto>(contactFromService);
                return Ok(contactReadDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("{contactId}/contactdetail/{contactDetailId}")]
        public async Task<ActionResult<ContactReadDto>> GetContactAndContactDetails(int contactId, int contactDetailId)
        {
            try
            {
                bool contactExists = await _contactBookService.ContactExists(contactId);

                if (!contactExists)
                {
                    return NotFound();
                }

                ContactDetail contactDetailFromService = await _contactBookService.GetContactDetail(contactId, contactDetailId);

                if (contactDetailFromService == null)
                {
                    return NotFound();
                }

                ContactReadDto contactReadDto = _mapper.Map<ContactReadDto>(contactDetailFromService.Contact);

                return Ok(contactReadDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<ContactReadDto>> CreateContact(ContactCreateDto createContactDto)
        {
            try
            {
                if (createContactDto == null)
                {
                    return BadRequest();
                }
                createContactDto.UpdatedDate = DateTime.UtcNow;

                Contact contact = _mapper.Map<Contact>(createContactDto);
                await _contactBookService.CreateContact(contact);
                await _contactBookService.SaveAsync();

                Contact recentlyAddedContact = await _contactBookService.FindAsync(contact.ContactDetails.First().ContactId);

                ContactReadDto contactReadDto = _mapper.Map<ContactReadDto>(recentlyAddedContact);

                return CreatedAtAction(nameof(GetContactById), new { id = contactReadDto.ContactId }, contactReadDto);

            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ContactReadDto>> UpdateContact(int id, ContactReadDto contactReadDto)
        {
            try
            {
                if (id != contactReadDto.ContactId)
                {
                    return BadRequest();
                }
                contactReadDto.UpdatedDate = DateTime.UtcNow;
                Contact contact = _mapper.Map<Contact>(contactReadDto);

                _contactBookService.UpdateContact(contact);
                await _contactBookService.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteContact(int id)
        {
            Contact contactFromService = await _contactBookService.GetContactById(id);
            if (contactFromService == null)
            {
                return BadRequest();
            }
            await _contactBookService.DeleteContact(contactFromService);
            await _contactBookService.SaveAsync();

            return NoContent();
        }
    }
}
