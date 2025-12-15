using Fundo.Applications.Application.Features.Loans.Commands.CreateLoan;
using Fundo.Applications.Application.Features.Loans.Commands.MakePayment;
using Fundo.Applications.Application.Features.Loans.Queries.GetLoanById;
using Fundo.Applications.Application.Features.Loans.Queries.GetLoans;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Controllers
{
    [Route("/loan")]
    public class LoanManagementController : Controller
    {
        private readonly IMediator _mediator;

        public LoanManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetLoansAsync()
        {
            var loans = await _mediator.Send(new GetLoansQuery());
            return Ok(loans);
        }

        [HttpGet("{id}", Name = nameof(GetLoanAsync))]
        public async Task<ActionResult> GetLoanAsync(string id)
        {
            var loan = await _mediator.Send(new GetLoanByIdQuery(id));
            return Ok(loan);
        }

        [HttpPost]
        public async Task<ActionResult> CreateLoanAsync([FromBody] CreateLoanCommand command)
        {
            var loan = await _mediator.Send(command);
            return CreatedAtRoute(nameof(GetLoanAsync), new { id = loan.Id }, loan);
        }

        [HttpPost("{id}/payment")]
        public async Task<ActionResult> MakePaymentAsync(string id, [FromBody] MakePaymentCommand paymentCommand)
        {
            if (id != paymentCommand.Id) return BadRequest("The loan id in the route does not match the loan id provided in the request body.");
            await _mediator.Send(paymentCommand);
            return NoContent();
        }
    }
}