using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ResearchManagement.Api.interfaces;

namespace ResearchManagement.Api.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinalReportTopicController : ControllerBase
    {
        private readonly IFinalReportRepository _finalReportRepository;
        public FinalReportTopicController(IFinalReportRepository finalReportRepository)
        {
            _finalReportRepository = finalReportRepository;
        }

        [HttpGet("get_pendingFinalReportTopic")]
        public async Task<IActionResult> GetPendingFinalReportTopic()
        {
            try
            {
                var result = await _finalReportRepository.GetPendingFinalReports();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet("get_detailFinalReportTopic/{FinalReportId}")]
        public async Task<IActionResult> GetDetailReportTopic(int FinalReportId)
        {
            Console.WriteLine("ID:" + FinalReportId);
            if (FinalReportId <= 0)
            {
                return BadRequest(new
                {
                    Message = "Id not valid"
                });
            }
            try
            {
                var result = await _finalReportRepository.GetDetailFinalReportDTOs(FinalReportId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{TopicId}/accept")]
        public async Task<IActionResult> Accept(int TopicId)
        {
            try
            {
                var result = await _finalReportRepository.AcceptFinalReportTopic(TopicId);
                if (!result)
                {
                    return BadRequest(
                        new
                        {
                            Message = "Đã xảy ra một số lỗi"
                        }
                    );
                }

                return Ok(new
                {
                    Message = "Thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new
                    {
                        Message = ex.Message
                    }
                );
            }
        }
        [HttpGet("{TopicId}/reject")]
        public async Task<IActionResult> Reject(int TopicId)
        {
            try
            {                
                Console.WriteLine("ID" + TopicId);

                var result = await _finalReportRepository.RejectFinalReportTopic(TopicId);
                if (!result)
                {
                    return BadRequest(
                        new
                        {
                            Message = "Đã xảy ra một số lỗi"
                        }
                    );
                }

                return Ok(new
                {
                    Message = "Thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new
                    {
                        Message = ex.Message
                    }
                );
            }
        }
    }
}