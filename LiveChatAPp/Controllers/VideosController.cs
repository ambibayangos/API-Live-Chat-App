using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LiveChatAPp.Model;
using LiveChatAPp.Helper;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using LiveChatAPp.DAL;

namespace LiveChatAPp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {



        public class VideoBuilder
        {
            public string URL { get; set; }
        }

        public class PlayerBuilder
        {
            public string name { get; set; }
        }


        private readonly liveChatAppContext _context;
        private IVideoRepository videoRepository;
        private readonly IMapper _mapper;




        public VideosController(liveChatAppContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
            this.videoRepository = new ScribrAPI.DAL.VideoRepository(new liveChatAppContext());


        }


        // GET: api/Videos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideo()
        {
            return await _context.Video.ToListAsync();
        }

        // GET: api/Videos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> GetVideo(int id)
        {
            var video = await _context.Video.FindAsync(id);

            if (video == null)
            {
                return NotFound();
            }

            return video;
        }

        // PUT: api/Videos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVideo(int id, Video video)
        {
            if (id != video.VideoId)
            {
                return BadRequest();
            }

            _context.Entry(video).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Videos
        [HttpPost]
        public async Task<ActionResult<Video>> PostVideo(VideoBuilder URL)
        {
            string videoID = YoutubeHelper.GetVideoLink(URL.URL);
            Video newVideo = YoutubeHelper.getVideoInfo(videoID);

            _context.Video.Add(newVideo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVideo", new { id = newVideo.VideoId }, newVideo);
        }


        // DELETE: api/Videos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Video>> DeleteVideo(int id)
        {
            var video = await _context.Video.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            _context.Video.Remove(video);
            await _context.SaveChangesAsync();

            return video;
        }

        //PUT with PATCH to handle isFavourite
        [HttpPatch("update/{id}")]
        public VideoDTO Patch(int id, [FromBody]JsonPatchDocument<VideoDTO> videoPatch)
        {
            //get original video object from the database
            Video originVideo = videoRepository.GetVideoByID(id);
            //use automapper to map that to DTO object
            VideoDTO videoDTO = _mapper.Map<VideoDTO>(originVideo);
            //apply the patch to that DTO
            videoPatch.ApplyTo(videoDTO);
            //use automapper to map the DTO back ontop of the database object
            _mapper.Map(videoDTO, originVideo);
            //update video in the database
            _context.Update(originVideo);
            _context.SaveChanges();
            return videoDTO;
        }


        private bool VideoExists(int id)
        {
            return _context.Video.Any(e => e.VideoId == id);
        }
    }
}
